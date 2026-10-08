# استخدام سيرفر Nginx خفيف عشان نعرض صفحة الـ HTML
FROM nginx:alpine

# نسخ ملف index.html بتاعنا لمكان التشغيل في Nginx
COPY index.html /usr/share/nginx/html/index.html

# السيرفر بيشتغل تلقائي على بورت 80
EXPOSE 80