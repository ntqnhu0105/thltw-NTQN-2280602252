$(document).ready(function () {
    // Hàm định dạng tiền Việt Nam
    function formatVnd(amount) {
        return amount.toLocaleString('vi-VN') + ' VNĐ';
    }

    // Hàm cập nhật tổng tiền cho một dòng
    function updateRowTotal(row) {
        var price = parseFloat(row.data('price'));
        var quantity = parseInt(row.find('.quantity-input').val());
        var total = price * quantity;
        row.find('.total').text(formatVnd(total));
        updateCartSummary();
    }

    // Hàm cập nhật tổng tiền toàn giỏ hàng
    function updateCartSummary() {
        var totalAll = 0;
        $('.cart-item').each(function () {
            var price = parseFloat($(this).data('price'));
            var quantity = parseInt($(this).find('.quantity-input').val());
            totalAll += price * quantity;
        });
        $('.total-all').text(formatVnd(totalAll));
    }

    // Xử lý nút tăng số lượng
    $('.qty-increase').click(function () {
        var input = $(this).siblings('.quantity-input');
        var newValue = parseInt(input.val()) + 1;
        input.val(newValue);
        var row = $(this).closest('.cart-item');
        updateRowTotal(row);
        $(this).closest('.quantity-form').submit();
    });

    // Xử lý nút giảm số lượng
    $('.qty-decrease').click(function () {
        var input = $(this).siblings('.quantity-input');
        var currentValue = parseInt(input.val());
        if (currentValue > 1) {
            input.val(currentValue - 1);
            var row = $(this).closest('.cart-item');
            updateRowTotal(row);
            $(this).closest('.quantity-form').submit();
        }
    });

    // Xử lý khi nhập tay số lượng
    $('.quantity-input').on('input', function () {
        var row = $(this).closest('.cart-item');
        var value = parseInt($(this).val());
        if (isNaN(value) || value < 1) $(this).val(1);
        updateRowTotal(row);
        $(this).closest('.quantity-form').submit();
    });
});