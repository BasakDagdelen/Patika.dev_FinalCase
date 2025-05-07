# Patika.dev_FinalCase

# 📊 Expense Management System

## 📌 Genel Bilgilendirme

Bu proje, saha personelinin masraf taleplerini girebildiği, yöneticilerin ise bu talepleri onaylayıp ödeme süreçlerini yönetebildiği bir **Masraf Takip Sistemi**dir. Projede rol bazlı yetkilendirme (Admin & Personel), JWT ile authentication, Clean Architecture, Entity Framework Core, Unit of Work gibi modern yazılım mimarisi ve teknolojileri kullanılmaktadır.

---

## ⚙️ Kullanılan Teknolojiler

| Teknoloji | Açıklama |
|----------|----------|
| .NET 8 / ASP.NET Core Web API | Ana API katmanı |
| Entity Framework Core | ORM ve veri işlemleri |
| SQL Server | Veritabanı |
| JWT (JSON Web Tokens) | Kimlik doğrulama ve yetkilendirme |
| AutoMapper | DTO ve Entity dönüşümleri |
| Swagger | API dokümantasyonu ve test |
| Clean Architecture | Katmanlı mimari |
| Repository & Unit of Work | Veri erişim desenleri |
| FluentValidation | Veri doğrulama işlemleri |

---

## 🧱 Katmanlar (Clean Architecture)

![image](https://github.com/user-attachments/assets/b0596b12-0b73-48e2-928f-cfbcb1acc5e7)

---

## 🔄 Uygulama Akışı

1. Kullanıcı sisteme kayıt olur veya giriş yapar (JWT Token alır).
2. Rolü `Personel` olan kullanıcı, masraf talebi oluşturur.
3. `Admin` rolü, tüm talepleri görür ve onaylama/reddetme işlemleri yapar.
4. Onaylanan talepler ödeme işlemine alınır (banka simülasyonu).
5. Kullanıcı masraf ve ödeme geçmişini görebilir.

---

## 📮 API Endpoint Listesi (Genel)

> Aşağıdaki endpoint'ler Swagger UI üzerinden test edilebilir.

---

## 🧱 BaseController

Tüm controller'ların kalıtım aldığı temel sınıftır. Ortak işlemleri ve kullanıcı bilgilerine erişimi sağlar.

### Özellikler

- `CurrentUserId`: JWT Token içerisinden kullanıcı ID'sini alır.
- `CurrentUserRole`: JWT Token içerisinden kullanıcı rolünü alır.
- `Success<T>(data, message)`: Başarılı durumlar için `ApiResponse<T>` döner.
- `Fail<T>(message, statusCode)`: Hatalı durumlar için `ApiResponse<T>` döner.

---

### 👤 Auth
- `POST /api/Auth/Login` – JWT Token al

### 👥 Users
- `GET /api/Users` – Tüm kullanıcıları listele (Admin)
- `GET /api/Users/{id}` – Kullancıı detaylarını getir
- `POST /api/Users` – Yeni kullanıcı oluştur
- `PUT /api/Users/{id}` – Kullanıcı güncelle
- `DELETE /api/Users/{id}` – Kullanıcı sil

### 💸 Expenses
- `GET /api/Expenses` – Tüm masrafları listele
- `GET /api/Expenses/{id}` – Masraf detayı getir
- `POST /api/Expenses` – Masraf talebi oluştur (Personel)
- `PUT /api/Expenses/{id}` – Masraf talebini güncelle
- `DELETE /api/Expenses/{id}` – Masraf talebini sil
- `GET /api/Expenses/active` – Kullanıcının aktif masrafları
- `GET /api/Expenses/history` – Kullanıcının geçmiş masrafları
- `PUT /api/Expenses/approve/{id}` – Talebi onayla (Admin)
- `PUT /api/Expenses/reject/{id}` – Talebi reddet (Admin)

### 🏦 Payments
- `GET /api/Payments` – Tüm ödeme geçmişi
- `GET /api/Payments/{id}` – Belirli ödeme detayları
- `POST /api/Payments/simulate/{expenseId}` – Ödeme simülasyonu başlat (Admin)
- `DELETE /api/Payments/{id}` – Ödemeyi sil


---





