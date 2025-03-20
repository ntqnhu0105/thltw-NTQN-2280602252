document.addEventListener("DOMContentLoaded", function () {
    // Hiệu ứng hover cho card
    document.querySelectorAll(".card").forEach(card => {
        card.addEventListener("mouseenter", () => card.classList.add("shadow-lg"));
        card.addEventListener("mouseleave", () => card.classList.remove("shadow-lg"));
    });

    // Lọc sản phẩm theo danh mục từ dropdown
    const categoryFilter = document.getElementById("categoryFilter");
    if (categoryFilter) {
        categoryFilter.addEventListener("change", function () {
            const selectedCategory = this.value.toLowerCase();
            document.querySelectorAll(".col").forEach(col => {
                const category = col.querySelector(".category-large").textContent.toLowerCase();
                col.style.display = (selectedCategory === "" || category === selectedCategory) ? "" : "none";
            });
        });
    } else {
        console.error("Không tìm thấy phần tử #categoryFilter");
    }
});