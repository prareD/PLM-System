$(document).ready(function () {
    $("#tagNumberInput").keyup(function () {
        var tagNumber = $(this).val().trim();
        tagNumber = tagNumber.replace(/[^A-Za-z0-9]/g, '');
        tagNumber = tagNumber.toUpperCase().substring(0, 8);
        $(this).val(tagNumber);
    });

    $("#checkInBtn").click(function () {
        var tagNumber = $("#tagNumberInput").val().trim();

        if (tagNumber === '') {
            showError("Please enter a tag number.");
            return;
        }

        var tagNumbers = Array.from($("#parkingTable tbody tr td:first-child")).map(td => td.innerText);
        if (tagNumbers.includes(tagNumber)) {
            showError("Car with the given tag number is already checked in.");
            return;
        }

        if (tagNumbers.length >= 15) {
            showError("Lots are full. No more check-ins allowed.");
            return;
        }

        $.ajax({
            url: "/Home/CheckIn",
            type: "POST",
            data: { tagNumber: tagNumber },
            success: function (response) {
                handleCheckInSuccess(response.cars);
                setTimeout(clearInputAndError, 2000);
            },
            error: function () {
                showError("An error occurred during check-in.");
            }
        });
    });
    var checkOutBtn = false;
    $("#checkOutBtn").click(function () {
        var tagNumber = $("#tagNumberInput").val().trim();
        if (tagNumber === '') {
            showError("Please enter a tag number.");
            return;
        }
        $.ajax({
            url: "/Home/CheckOut",
            type: "POST",
            data: { tagNumber: tagNumber },
            success: function (response) {
                deleteEntryFromParkingTable(tagNumber);
                handleCheckOutSuccess(response.totalAmount, response.cars);
                checkOutBtn = true;
                setTimeout(clearInputAndError, 2000);
            },
            error: function () {
                showError("An error occurred during check-out.");
            }
        });
    });
    $("#statsbtn").click(function () {
        $.ajax({
            url: "/Home/Stats",
            type: "GET",
            success: function (response) {
                try {
                    updateStatsModal(response.stats, response.cars);
                    $("#statsModal").show();
                } catch (error) {
                    showError("Invalid response format for parking data.");
                }
                setTimeout(clearInputAndError, 2000);
            },
            error: function () {
                showError("An error occurred while retrieving stats.");
            }
        });
    });

    $("#closeModal").click(function () {
        $("#statsModal").hide();
    });

    function showError(message) {
        $("#errorMessage").text(message);
    }

    function handleCheckInSuccess(cars) {
        updateParkingTable(cars);
        showError("Car checked in successfully.");
        if ($("#parkingTable tbody tr").length >= 15) {
            $("#checkInBtn").text("Lots are full").prop("disabled", true);
        }
    } 
    function handleCheckOutSuccess(totalAmount,cars) {
        var tagNumber = $("#tagNumberInput").val().trim();
        var foundCar = cars.find(function (car) {
            return car.tagNumber === tagNumber;
        });
        if (foundCar) {
            var checkInTime = foundCar.checkInTime;
        }
        var checkOutTime = new Date();
        showError("Car checked out successfully. Total amount: " + totalAmount);
        deleteEntryFromParkingTable(tagNumber);
        checkOutBtn = true;
        updateParkingTable(cars);
        $("#parkingTable tbody tr:contains(" + tagNumber + ") td:last-child").text(elapsedTime);
        setTimeout(clearInputAndError, 2000);
    }

    function deleteEntryFromParkingTable(tagNumber) {
        $("#parkingTable tbody tr").each(function () {
            var currentTagNumber = $(this).find("td:first-child").text().trim();
            if (currentTagNumber === tagNumber) {
                $(this).remove();
                var index = cars.findIndex(function (car) {
                    return car.tagNumber === tagNumber;
                });
                if (index !== -1) {
                    cars.splice(index, 1);
                }
            }
        });
    }
  
    function clearInputAndError() {
        $("#tagNumberInput").val("");
        $("#errorMessage").text("");
    }

    function updateStatsModal(stats, cars) {
        var totalCarsEntered = 15 - parseInt(stats.availableSpots);
        var totalHours = 10;
        var totalRevenue = totalHours * 15;
        var averageRevenuePerCar = totalRevenue / totalCarsEntered;

        $("#spotsAvailable").text(stats.availableSpots);
        $("#todaysRevenue").text(totalRevenue.toFixed(2));
        $("#avgCarsPerDay").text(totalCarsEntered);
        $("#avgRevenuePerDay").text(averageRevenuePerCar.toFixed(2));
    }

    function updateParkingTable(cars) {
        var tableBody = $("#parkingTable tbody");
        tableBody.empty();

        var currentTime = new Date(); //newL
        cars.forEach(function (car) {
            var checkInTime = new Date(car.checkInTime);
            var elapsedHours = CalculateElapsedTime(checkInTime, currentTime); //newL
            var row =
                `<tr>
                <td>${car.tagNumber}</td>
                <td>${car.spotId}</td>
                <td>${formatDate(checkInTime)}</td>
                <td>${formatDate(car.checkoutTime) }</td>
                <td class="elapsed-time">${elapsedHours.toFixed(2)}</td>
            </tr>`;
            tableBody.append(row); //<td>${elapsedTime}</td>
        });
    }
    function formatDate(date) {
        if (!isDate(date)) { return "-"}
        return date.toLocaleString('en-US', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit'
        });
    }

    function isDate(value) {
        return value instanceof Date && !isNaN(value.getTime());
    }

    // Function to calculate elapsed time
    function CalculateElapsedTime(checkInTime, currentTime) {
        var elapsedMilliseconds = currentTime - checkInTime;
        var elapsedHours = elapsedMilliseconds / (1000 * 60 * 60);
        return elapsedHours;
    }

    // Function to update elapsed times
    function updateElapsedTimes() {
        $(".elapsed-time").each(function () {
            var tagNumber = $(this).closest("tr").find("td:first-child").text();
            var car = cars.find(function (c) {
                return c.tagNumber === tagNumber;
            });

            if (car) {
                var checkInTimeString = $("#parkingTable tbody tr:contains(" + tagNumber + ") td:nth-child(3)").text().trim();
                var checkInTime = new Date(checkInTimeString);
                var currentTime = new Date();
                var elapsedHours = CalculateElapsedTime(checkInTime, currentTime);
                $(this).text(elapsedHours.toFixed(2));
            }
        });
    }
    updateElapsedTimes();
    // Schedule the function to run every 30 seconds
    setInterval(updateElapsedTimes, 20 * 1000); // 30 seconds in milliseconds
});
