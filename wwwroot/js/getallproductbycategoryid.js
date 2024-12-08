$(document).ready(function () {
    // Function to fetch products by category ID
    $('#filterForm').on('submit', function (e) {
        e.preventDefault();

        var categoryId = $('#categoryIdInput').val();
        var apiUrl = `https://localhost:7179//api/Product/category/${categoryId}`;

        // Clear previous product data
        $('#productTableBody').empty();

        // Fetch data from the API
        $.ajax({
            url: apiUrl,
            type: 'GET',
            success: function (data) {
                // Populate the table with the data
                data.forEach(function (product) {
                    var row = `
                        <tr>
                            <td>${product.productId}</td>
                            <td>${product.productName}</td>
                            <td>${product.supplierId}</td>
                            <td>${product.categoryId}</td>
                            <td>${product.quantityPerUnit}</td>
                            <td>${product.unitPrice}</td>
                            <td>${product.unitsInStock}</td>
                            <td>${product.unitsOnOrder}</td>
                            <td>${product.reorderLevel}</td>
                            <td>${product.discontinued}</td>
                        </tr>
                    `;
                    $('#productTableBody').append(row);
                });
            },
            error: function () {
                alert('Error fetching products.');
            }
        });
    });
});
