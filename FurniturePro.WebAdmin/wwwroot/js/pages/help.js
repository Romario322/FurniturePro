// wwwroot/js/pages/help.js

document.addEventListener('DOMContentLoaded', () => {
    const navLinks = document.querySelectorAll('.help-nav-link');
    const sections = document.querySelectorAll('.help-section');
    const scrollContainer = document.getElementById('helpContentScroll');

    // 1. Плавная прокрутка при клике
    navLinks.forEach(link => {
        link.addEventListener('click', (e) => {
            e.preventDefault();
            const targetId = link.getAttribute('href').substring(1);
            const targetSection = document.getElementById(targetId);

            if (targetSection && scrollContainer) {
                // Вычисляем позицию внутри контейнера
                const topPos = targetSection.offsetTop - scrollContainer.offsetTop;

                scrollContainer.scrollTo({
                    top: topPos,
                    behavior: 'smooth'
                });

                // Обновляем активный класс вручную
                navLinks.forEach(n => n.classList.remove('active'));
                link.classList.add('active');
            }
        });
    });

    // 2. ScrollSpy (Подсветка при скролле)
    if (scrollContainer) {
        scrollContainer.addEventListener('scroll', () => {
            let current = '';

            sections.forEach(section => {
                const sectionTop = section.offsetTop - scrollContainer.offsetTop;
                const sectionHeight = section.clientHeight;

                // Если секция находится в верхней трети вьюпорта контейнера
                if (scrollContainer.scrollTop >= (sectionTop - 100)) {
                    current = section.getAttribute('id');
                }
            });

            navLinks.forEach(link => {
                link.classList.remove('active');
                if (link.getAttribute('href') === '#' + current) {
                    link.classList.add('active');
                }
            });
        });
    }
});