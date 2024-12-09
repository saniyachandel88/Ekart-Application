$('#submitProduct').click(function () {
    // Collect the form data
    var productName = $('#productName').val();
    var supplierId = $('#supplierId').val();
    var categoryId = $('#categoryId').val();
    var quantityPerUnit = $('#quantityPerUnit').val();
    var unitPrice = $('#unitPrice').val();
    var unitsInStock = $('#unitsInStock').val();
    var unitsOnOrder = $('#unitsOnOrder').val();
    var reorderLevel = $('#reorderLevel').val();
    var discontinued = $('#discontinued').val() === "true";

    // Create the request body
    var productDto = {
        ProductName: productName,
        SupplierId: supplierId,
        CategoryId: categoryId,
        QuantityPerUnit: quantityPerUnit,
        UnitPrice: unitPrice,
        UnitsInStock: unitsInStock,
        UnitsOnOrder: unitsOnOrder,
        ReorderLevel: reorderLevel,
        Discontinued: discontinued
    };

    // Send AJAX request
    $.ajax({
        url: 'https://localhost:7179/api/Product/edit/' + $('#productId').val(), // Ensure you pass the correct ID
        type: 'PATCH',
        contentType: 'application/json',
        data: JSON.stringify(productDto),
        success: function (response) {
            // Handle success
            alert("Product updated successfully!");
        },
        error: function (xhr, status, error) {
            // Handle error
            alert("Error: " + xhr.responseText);
        }
    });
});
