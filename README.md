# Patika.dev_FinalCase

# 📊 Expense Management System

## 📌 Genel Bilgilendirme

Bu proje, saha personelinin masraf taleplerini girebildiği, yöneticilerin ise bu talepleri onaylayıp ödeme süreçlerini yönetebildiği bir **Masraf Takip Sistemi**dir. Projede rol bazlı yetkilendirme (Admin & Personel), JWT ile authentication, Clean Architecture, Entity Framework Core, Unit of Work gibi modern yazılım mimarisi ve teknolojileri kullanılmaktadır.

---

## 🚀 Proje Kurulumu ve Çalıştırma

### 📦 1. Projeyi Klonla

```bash
git clone https://github.com/kullanici-adi/Expense-Management-System.git
cd Expense-Management-System
```

### 🧰 2. Gereksinimler

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* [Visual Studio 2022+](https://visualstudio.microsoft.com/) veya [VS Code](https://code.visualstudio.com/)
* [Postman](https://www.postman.com/) (isteğe bağlı)

---

## ⚙️ 3. Yapılandırma

### 🔌 Veritabanı Ayarı

`appsettings.json` ya da `appsettings.Development.json` içinde `ConnectionStrings` alanını düzenle:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ExpenseDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Alternatif olarak:

```json
"Server=localhost;Database=ExpenseDb;User Id=sa;Password=yourPassword;"
```

### 🔐 JWT Ayarları

```json
"JwtSettings": {
  "SecretKey": "your-super-secret-key",
  "Issuer": "ExpenseApp",
  "Audience": "ExpenseUsers",
  "AccessTokenExpirationMinutes": 60
}
```

### 🏠 Veritabanı Kurulumu

EF Core Migration kullanıyorsan:

```bash
dotnet ef database update
```

EF Core migration yoksa, `Database/Script.sql` dosyasını çalıştırarak manuel olarak veritabanını oluşturabilirsin.

---

## ▶️ Uygulamayı Başlat

```bash
dotnet run --project ExpenseManagement.Api
```

Ardından API Swagger arayüzünü ziyaret et:

```
https://localhost:5001/swagger
```

---

## 👤 Varsayılan Admin Kullanıcı

| Alan  | Değer                                         |
| ----- | --------------------------------------------- |
| Email | [admin@example.com](mailto:admin@example.com) |
| Şifre | Admin123\*                                    |

> İlk kullanıcıyı manuel olarak veritabanına eklemen gerekebilir 
---

## 🧰 API Endpoint Listesi

Tüm controller'lar REST mimarisine uygun olarak GET, GET by ID, POST, PUT ve DELETE işlemlerini destekler.

### 🔐 AuthController

| HTTP | Endpoint             | Açıklama |
| ---- | -------------------- | -------- |
| POST | `/api/Auth/login`    | Giriş    |
| POST | `/api/Auth/register` | Kayıt    |

### 📟 ExpenseController

| HTTP   | Endpoint            | Açıklama                        |
| ------ | ------------------- | ------------------------------- |
| GET    | `/api/Expense`      | Tüm masrafları getir (Personel) |
| GET    | `/api/Expense/{id}` | Belirli masrafı getir           |
| POST   | `/api/Expense`      | Yeni masraf oluştur             |
| PUT    | `/api/Expense/{id}` | Masraf güncelle                 |
| DELETE | `/api/Expense/{id}` | Masraf sil                      |

### 📂 ExpenseCategoryController

| HTTP   | Endpoint                    | Açıklama                      |
| ------ | --------------------------- | ----------------------------- |
| GET    | `/api/ExpenseCategory`      | Tüm kategorileri getir        |
| GET    | `/api/ExpenseCategory/{id}` | Kategori detayını getir       |
| POST   | `/api/ExpenseCategory`      | Yeni kategori oluştur (Admin) |
| PUT    | `/api/ExpenseCategory/{id}` | Kategori güncelle (Admin)     |
| DELETE | `/api/ExpenseCategory/{id}` | Kategori sil (Admin)          |

### 💳 PaymentController

| HTTP | Endpoint            | Açıklama                      |
| ---- | ------------------- | ----------------------------- |
| GET  | `/api/Payment`      | Ödeme geçmişini getir (Admin) |
| GET  | `/api/Payment/{id}` | Belirli ödeme detayını getir  |
| POST | `/api/Payment/pay`  | Ödeme yap (Admin)             |

### 🏦 BankAccountController

| HTTP   | Endpoint                | Açıklama                            |
| ------ | ----------------------- | ----------------------------------- |
| GET    | `/api/BankAccount`      | Kullanıcının banka hesabını getirir |
| POST   | `/api/BankAccount`      | Yeni banka hesabı oluşturur         |
| PUT    | `/api/BankAccount/{id}` | Hesap bilgilerini günceller         |
| DELETE | `/api/BankAccount/{id}` | Hesabı siler                        |

---

## 🧱 Mimari Yapı

Bu proje **Clean Architecture** prensiplerine göre geliştirilmiştir.

### 📁 Katmanlar

| Katman           | Açıklama                                |
| ---------------- | --------------------------------------- |
| `Api`            | API controller’lar                      |
| `Application`    | DTO, servis, business kurallar          |
| `Domain`         | Entity tanımları, arayüzler             |
| `Infrastructure` | Jwt konfigürasyonu, yardımcı servisler  |
| `Persistence`    | EF Core, Dapper, Repository, UnitOfWork |

---

## 🔒 Yetkilendirme

* JWT ile kimlik doğrulama yapılır.
* `[Authorize]` ve `[Authorize(Roles = "Admin")]` gibi attribute’lar kullanılır.
* Personel sadece kendi masraflarını görebilir.
* Admin tüm kayıtları yönetebilir, onaylama ve ödeme işlemlerini gerçekleştirir.

---

## 📌 Ekstra Özellikler

* Generic Repository & Unit of Work Pattern
* DTO – Entity Mapping (AutoMapper)
* Exception Handling ve Custom Response Format (ApiResponse)
* Swagger ile dokümantasyon ve test

---

## 🤝 Katkıda Bulunmak

Pull request’ler ve issue’lar her zaman memnuniyetle karşılanır!

---

## 🖲 Lisans

Bu proje MIT lisansı ile lisanslanmıştır.


![Ekran görüntüsü 2025-05-07 044728](https://github.com/user-attachments/assets/b9bc1007-43f1-482d-ba52-19da466cec1b)

![Ekran görüntüsü 2025-05-07 045703](https://github.com/user-attachments/assets/b1de4351-45fa-48a8-9091-febc1fb4dc1a)




