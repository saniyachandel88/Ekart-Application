const apiUrlById = "https://localhost:7179/api/Supplier/";

$(document).ready(function () {
    // Event handler for "Get Supplier By ID" button
    $("#getSupplierByIdBtn").click(function () {
        let supplierId = $("#supplierIdInput").val().trim();

        // Ensure the supplier ID input is valid
        if (supplierId && !isNaN(supplierId)) {
            console.log(`Fetching supplier with ID: ${supplierId}`);
            getSupplierById(supplierId);
        } else {
            alert("Please enter a valid Supplier ID.");
        }
    });

    function getSupplierById(id) {
        // Clear the table before loading new data
        $("#supplierTableBody").html(`
            <tr>
                <td colspan="10" class="text-center alert alert-info">Loading...</td>
            </tr>
        `);

        // Get the token from localStorage
        const token = localStorage.getItem("authToken");

        // Check if the token is available
        if (!token) {
            alert("You must log in to access this feature.");
            window.location.href = "/Views/include/login.html";
            return;
        }
       

        $.ajax({
            url: `${apiUrlById}${id}`,
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}` // Attach the token in Authorization header
            },
            success: function (response) {
                console.log("API Response:", response);

                if (!response || response.length === 0) {
                    $("#supplierTableBody").html(`
                        <tr>
                            <td colspan="10" class="text-center alert alert-info">
                                No supplier found with this ID.
                            </td>
                        </tr>
                    `);
                    return;
                }

                let supplier = response[0]; // As it returns an array, get the first element
                let supplierRow = `
                    <tr>
                        <td>${sanitize(supplier.companyName)}</td>
                        <td>${sanitize(supplier.contactName) || "N/A"}</td>
                        <td>${sanitize(supplier.contactTitle) || "N/A"}</td>
                        <td>${sanitize(supplier.address) || "N/A"}</td>
                        <td>${sanitize(supplier.city) || "N/A"}</td>
                        <td>${sanitize(supplier.region) || "N/A"}</td>
                        <td>${sanitize(supplier.postalCode) || "N/A"}</td>
                        <td>${sanitize(supplier.country) || "N/A"}</td>
                        <td>${sanitize(supplier.phone) || "N/A"}</td>
                        <td>${sanitize(supplier.fax) || "N/A"}</td>
                    </tr>
                `;
                $("#supplierTableBody").html(supplierRow);
            },
            error: function (xhr, status, error) {
                console.error("Error fetching supplier:", error);
                console.error("XHR Details:", xhr);

                const errorMsg = xhr.responseJSON?.message || "Failed to fetch supplier. Please try again.";
                $("#supplierTableBody").html(`
                    <tr>
                        <td colspan="10" class="text-center alert alert-danger">
                            ${errorMsg}
                        </td>
                    </tr>
                `);
            }
        });
    }

    // Function to sanitize HTML input to prevent XSS
    function sanitize(str) {
        return str ? str
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;")
            : str;
    }
});
