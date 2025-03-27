$(document).ready(function () {
    $('.product-card').hover(
        function () {
            $(this).find('.price-value').css({
                'color': '#ff073a',
                'font-size': '1.4rem',
                'transition': 'all 0.3s ease'
            });
        },
        function () {
            $(this).find('.price-value').css({
                'color': '#dc3545',
                'font-size': '1.3rem',
                'transition': 'all 0.3s ease'
            });
        }
    );
});