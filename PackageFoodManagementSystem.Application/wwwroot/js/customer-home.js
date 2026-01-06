
//// customer-home.js
//// Works with your existing HTML—no CSS classes required.
//// Adds: logo & hero image pop-in, hero text hover wobble + cursor tilt,
//// navbar link hover animations, and a subtle pulse for the cart badge.

//(function () {
//    // Helper: apply inline transition safely
//    function setTransition(el, props, duration = 300, easing = 'ease') {
//        const existing = el.style.transition;
//        const list = Array.isArray(props) ? props : [props];
//        const newTx = list.map(p => `${p} ${duration}ms ${easing}`).join(', ');
//        el.style.transition = existing ? `${existing}, ${newTx}` : newTx;
//    }

//    // Helper: stagger animation
//    function stagger(fn, delay, ...elements) {
//        elements.forEach((el, i) => {
//            if (!el) return;
//            setTimeout(() => fn(el), i * delay);
//        });
//    }

//    document.addEventListener('DOMContentLoaded', function () {
//        const logo = document.querySelector('.app-logo');
//        const heroImg = document.querySelector('.hero-image');
//        const heroText = document.querySelector('.hero-text');
//        const navbarLinks = Array.from(document.querySelectorAll('.navbar .nav-link'));
//        const cartBadge = document.querySelector('.navbar .badge');

//        // -----------------------------
//        // 1) Pop-in on load: logo & hero image
//        // -----------------------------
//        function popIn(el, scaleFrom = 0.92, duration = 500, delay = 0) {
//            if (!el) return;
//            el.style.opacity = '0';
//            el.style.transform = `scale(${scaleFrom})`;
//            setTransition(el, ['opacity', 'transform'], duration, 'cubic-bezier(.2,.7,.2,1)');
//            setTimeout(() => {
//                el.style.opacity = '1';
//                el.style.transform = 'scale(1)';
//            }, delay);
//        }

//        stagger(popIn, 150, logo, heroImg);

//        // -----------------------------
//        // 2) Hero text: fade-in, hover wobble & cursor tilt
//        // -----------------------------
//        if (heroText) {
//            // Initial fade-up
//            heroText.style.opacity = '0';
//            heroText.style.transform = 'translateY(8px)';
//            setTransition(heroText, ['opacity', 'transform'], 550, 'ease-out');
//            setTimeout(() => {
//                heroText.style.opacity = '1';
//                heroText.style.transform = 'translateY(0)';
//            }, 120);

//            // Hover wobble (CSS-free via JS)
//            let wobbleTimer = null;
//            heroText.addEventListener('mouseenter', () => {
//                if (wobbleTimer) clearInterval(wobbleTimer);
//                const sequence = [
//                    { r: -1.2, x: -1 },
//                    { r: 1.2, x: 1 },
//                    { r: -0.6, x: -0.5 },
//                    { r: 0, x: 0 }
//                ];
//                let idx = 0;
//                setTransition(heroText, 'transform', 200, 'ease');
//                wobbleTimer = setInterval(() => {
//                    const s = sequence[idx];
//                    heroText.style.transform = `translateY(0) rotate(${s.r}deg) translateX(${s.x}px)`;
//                    idx++;
//                    if (idx >= sequence.length) {
//                        clearInterval(wobbleTimer);
//                        wobbleTimer = null;
//                        heroText.style.transform = 'translateY(0) rotate(0deg) translateX(0)';
//                    }
//                }, 120);
//            });

//            // Cursor-based tilt (disabled on touch devices)
//            const isTouch = 'ontouchstart' in window || navigator.maxTouchPoints > 0;
//            if (!isTouch) {
//                let rafId = null;
//                const maxTilt = 6; // degrees
//                function onMove(e) {
//                    const rect = heroText.getBoundingClientRect();
//                    const cx = rect.left + rect.width / 2;
//                    const cy = rect.top + rect.height / 2;
//                    const dx = (e.clientX - cx) / (rect.width / 2);
//                    const dy = (e.clientY - cy) / (rect.height / 2);
//                    const rotY = Math.max(-1, Math.min(1, dx)) * maxTilt;
//                    const rotX = Math.max(-1, Math.min(1, dy)) * -maxTilt;
//                    if (rafId) cancelAnimationFrame(rafId);
//                    rafId = requestAnimationFrame(() => {
//                        heroText.style.transform = `perspective(600px) rotateX(${rotX}deg) rotateY(${rotY}deg)`;
//                    });
//                }
//                function resetTilt() {
//                    if (rafId) cancelAnimationFrame(rafId);
//                    heroText.style.transform = 'translateY(0)';
//                }
//                heroText.addEventListener('mousemove', onMove);
//                heroText.addEventListener('mouseleave', resetTilt);
//            }
//        }

//        // -----------------------------
//        // 3) Navbar links: color + underline slide-in (JS-driven)
//        // -----------------------------
//        // We’ll create a temporary underline element that slides in/out
//        navbarLinks.forEach(link => {
//            link.style.position = link.style.position || 'relative';
//            link.style.willChange = 'color';

//            function createUnderline() {
//                const u = document.createElement('span');
//                u.className = 'js-underline';
//                u.style.position = 'absolute';
//                u.style.left = '0';
//                u.style.bottom = '-2px';
//                u.style.height = '2px';
//                u.style.width = '0%';
//                u.style.background = 'linear-gradient(90deg, #ff6b6b, #4ecdc4)';
//                u.style.transition = 'width 220ms ease';
//                u.style.pointerEvents = 'none';
//                return u;
//            }

//            let underline = null;

//            link.addEventListener('mouseenter', () => {
//                link.style.transition = 'color 180ms ease';
//                link.style.color = '#111827';
//                if (!underline) {
//                    underline = createUnderline();
//                    link.appendChild(underline);
//                }
//                requestAnimationFrame(() => {
//                    underline.style.width = '100%';
//                });
//            });

//            link.addEventListener('mouseleave', () => {
//                link.style.color = '';
//                if (underline) {
//                    underline.style.width = '0%';
//                    // remove after animation ends
//                    setTimeout(() => {
//                        if (underline && underline.parentNode) {
//                            underline.parentNode.removeChild(underline);
//                        }
//                        underline = null;
//                    }, 240);
//                }
//            });
//        });

//        // -----------------------------
//        // 4) Cart badge: gentle pulse (JS loop)
//        // -----------------------------
//        if (cartBadge) {
//            let t = 0;
//            cartBadge.style.transformOrigin = 'center';
//            function pulse() {
//                // Create a subtle scale oscillation
//                t += 0.04; // speed
//                const s = 1 + Math.sin(t) * 0.06; // amplitude
//                cartBadge.style.transform = `scale(${s.toFixed(3)})`;
//                requestAnimationFrame(pulse);
//            }
//            pulse();
//        }

//        // -----------------------------
//        // 5) Optional: menu cards lift on hover (no CSS needed)
//        // -----------------------------
//        const cards = Array.from(document.querySelectorAll('.menu-card'));
//        cards.forEach(card => {
//            setTransition(card, ['transform', 'box-shadow'], 180, 'ease');
//            card.addEventListener('mouseenter', () => {
//                card.style.transform = 'translateY(-4px)';
//                card.style.boxShadow = '0 10px 22px rgba(0,0,0,.08)';
//            });
//            card.addEventListener('mouseleave', () => {
//                card.style.transform = 'translateY(0)';
//                card.style.boxShadow = '0 6px 18px rgba(0,0,0,.06)';
//            });
//        });

//        // -----------------------------
//        // 6) Respect prefers-reduced-motion
//        // -----------------------------
//        const prm = window.matchMedia('(prefers-reduced-motion: reduce)');
//        if (prm.matches) {
//            // Disable animations by clearing transitions and transforms
//            [logo, heroImg, heroText, cartBadge, ...navbarLinks, ...cards]
//                .filter(Boolean)
//                .forEach(el => {
//                    el.style.transition = 'none';
//                    el.style.transform = 'none';
//                });
//        }
//    });
//})();
//``
