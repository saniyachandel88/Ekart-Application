const apiUrlById = "https://localhost:7179/api/Product/category/";

$(document).ready(function () {
    // Event handler for "Get Products By Category" button
    $("#getProductsByCategoryBtn").click(function () {
        let categoryId = $("#categoryIdInput").val().trim();

        // Ensure the category ID input is valid
        if (categoryId && !isNaN(categoryId)) {
            console.log(`Fetching products with Category ID: ${categoryId}`);
            getProductsByCategoryId(categoryId);
        } else {
            alert("Please enter a valid Category ID.");
        }
    });

    function getProductsByCategoryId(id) {
        // Clear the table before loading new data
        $("#productTableBody").html(`
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
            url: `${apiUrlById}${id}`, // Use updated API URL
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}` // Attach the token in Authorization header
            },
            success: function (response) {
                console.log("API Response:", response);

                if (!response || response.length === 0) {
                    $("#productTableBody").html(`
                        <tr>
                            <td colspan="10" class="text-center alert alert-info">
                                No products found for this category.
                            </td>
                        </tr>
                    `);
                    return;
                }

                // Populate table with products
                let productsRow = "";
                response.forEach(product => {
                    productsRow += `
                        <tr>
                            <td>${sanitize(product.productId) || "N/A"}</td>
                            <td>${sanitize(product.productName) || "N/A"}</td>
                            <td>${sanitize(product.supplierId) || "N/A"}</td>
                            <td>${sanitize(product.categoryId) || "N/A"}</td>
                            <td>${sanitize(product.quantityPerUnit) || "N/A"}</td>
                            <td>${product.unitPrice ? product.unitPrice.toFixed(2) : "N/A"}</td>
                            <td>${sanitize(product.unitsInStock) || "N/A"}</td>
                            <td>${sanitize(product.unitsOnOrder) || "N/A"}</td>
                            <td>${sanitize(product.reorderLevel) || "N/A"}</td>
                            <td>${product.discontinued ? "Yes" : "No"}</td>
                        </tr>
                    `;
                });
                $("#productTableBody").html(productsRow);
            },
            error: function (xhr, status, error) {
                console.error("Error fetching products:", error);
                console.error("XHR Details:", xhr);

                const errorMsg = xhr.responseJSON?.message || "Failed to fetch products. Please try again.";
                $("#productTableBody").html(`
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
