# استخدام بيئة الـ SDK عشان نبني المشروع
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# نسخ ملفات المشروع وعمل Restore للمكتبات
COPY *.csproj ./
RUN dotnet restore

# نسخ باقي الملفات وعمل Publish للنسخة النهائية
COPY . ./
RUN dotnet publish -c Release -o out

# استخدام بيئة التشغيل فقط عشان يكون حجم الكونتينر صغير
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# تشغيل الـ API
ENTRYPOINT ["dotnet", "backend.dll"]