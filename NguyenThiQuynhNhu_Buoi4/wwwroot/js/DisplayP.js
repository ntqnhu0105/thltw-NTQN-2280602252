<script>
    document.addEventListener("DOMContentLoaded", function () {
    const backButton = document.getElementById("backButton");
    backButton.addEventListener("click", function (event) {
        event.preventDefault();
    alert("Returning to product list!");
    window.location.href = backButton.getAttribute("href");
    });
});
</script>