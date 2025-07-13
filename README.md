# Dynamic Chart

**Dinamik Chart Görselleştirme Uygulaması**

Bu proje, veritabanı objelerinden (View, Stored Procedure, Function) veri çekerek dinamik grafikler oluşturan web tabanlı bir uygulamadır. Kullanıcılar veritabanı bağlantısı kurarak, veri objelerini seçip farklı chart türlerinde görselleştirme yapabilirler.

## 🚀 Özellikler

- **Dinamik Veritabanı Bağlantısı**: Kullanıcı arayüzünden veritabanı bağlantı bilgileri girme
- **Çoklu Veri Objesi Desteği**: View, Stored Procedure ve Function desteği
- **Interaktif Chart Türleri**: Line, Bar ve Radar chart desteği
- **Veri Haritalama**: Kullanıcı dostu dropdown ile X/Y ekseni eşleştirme
- **Responsive Tasarım**: Modern, responsive web arayüzü
- **JWT Authentication**: Güvenli API erişimi
- **RESTful API**: .NET Core 9 Minimal API

## 🛠️ Kullanılan Teknolojiler

### Backend
- **.NET Core 9.0** - Web API Framework
- **ADO.NET** - ORM
- **Minimal API** - Lightweight API endpoints
- **JWT Bearer Authentication** - API güvenliği
- **Microsoft SQL Server** - Veritabanı

### Frontend
- **HTML5** - Markup
- **CSS3** - Styling (Bootstrap 5.3)
- **JavaScript (Vanilla)** - Client-side logic
- **Chart.js 3.9.1** - Chart visualization
- **Bootstrap 5.3** - UI Framework
- **Font Awesome 6.4** - Icons

### Veritabanı
- **Microsoft SQL Server** - Primary database
- **MSSQL Server Management Studio** - Database management

## 📋 Sistem Gereksinimleri

- **.NET 9.0 SDK** veya üzeri
- **Microsoft SQL Server** 2019 veya üzeri
- **Visual Studio 2022** (önerilen)

## 🔧 Kurulum Adımları

### 1. Projeyi Klonlayın
```bash
git clone https://github.com/muhammetseyda/DinamicCharts.git
cd DinamicCharts
```

### 2. Örnek Veritabanı Kurulumu
Microsoft SQL Server'da `dinamicChart` veritabanını oluşturun ve aşağıdaki tabloları ekleyin:

```sql
-- Customer tablosu
CREATE TABLE [dbo].[customer](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [customerid] [varchar](50) NOT NULL,
    [first_name] [varchar](100) NOT NULL,
    [last_name] [varchar](100) NOT NULL,
    [email] [varchar](255) NOT NULL,
    [phonenumber] [varchar](20) NULL,
    CONSTRAINT [PK_customer] PRIMARY KEY ([id])
)

-- Product tablosu
CREATE TABLE [dbo].[product](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [productid] [varchar](50) NOT NULL,
    [name] [varchar](200) NOT NULL,
    [price] [decimal](10, 2) NOT NULL,
    [stock] [int] NOT NULL,
    CONSTRAINT [PK_product] PRIMARY KEY ([id])
)

-- Orders tablosu
CREATE TABLE [dbo].[orders](
    [id] [int] IDENTITY(1,1) NOT NULL,
    [orderid] [varchar](50) NOT NULL,
    [productid] [varchar](50) NOT NULL,
    [customerid] [varchar](50) NOT NULL,
    [quantity] [int] NOT NULL,
    [orderdate] [datetime] NOT NULL,
    CONSTRAINT [PK_orders] PRIMARY KEY ([id])
)
```

### 3. Views, Stored Procedures ve Functions
Aşağıdaki veritabanı objelerini oluşturun:

#### Views
- `vw_ProductSummary` - Ürün özet bilgileri
- `vw_OrderSummary` - Sipariş özet bilgileri  
- `vw_CustomerOrderStats` - Müşteri sipariş istatistikleri

#### Stored Procedures
- `sp_GetAllProducts` - Tüm ürünleri getir
- `sp_GetAllOrders` - Tüm siparişleri getir
- `sp_GetAllCustomers` - Tüm müşterileri getir
- `sp_MostOrderedProducts` - En çok sipariş verilen ürünler
- `sp_TopCustomers` - En iyi müşteriler

#### Functions
- `fn_GetAllProducts` - Tüm ürünleri getir (function)
- `fn_GetAllOrders` - Tüm siparişleri getir (function)
- `fn_GetAllCustomers` - Tüm müşterileri getir (function)
- `fn_ProductOrderStats` - Ürün sipariş istatistikleri
- `fn_CustomerStats` - Müşteri istatistikleri

### 4. Bağımlılıkları Yükleyin
```bash
dotnet restore
```

### 5. Uygulamayı Çalıştırın
```bash
dotnet run
```

## 🎯 Kullanım Kılavuzu
### Login
 - Aşağıda beliritlen bilgiler ile giriş yapınız.

### 1. Veritabanı Bağlantısı
- Ana sayfada veritabanı bağlantı bilgilerini girin
- **Host**: `localhost\SQLEXPRESS`
- **Database**: `dinamicChart`
- **Username**: Veritabanı kullanıcı adı
- **Password**: Veritabanı şifresi

### 2. Veri Objesi Seçimi
- Bağlantı kurulduktan sonra mevcut Views, Stored Procedures ve Functions listelenir
- İstediğiniz veri objesini seçin

### 3. Chart Tipi Seçimi
- **Line Chart**: Zaman serisi veriler için ideal
- **Bar Chart**: Kategorik veriler için uygun
- **Radar Chart**: Çok boyutlu veri karşılaştırması

### 4. Veri Haritalama
- X Ekseni: Kategori/Label alanını seçin
- Y Ekseni: Sayısal değer alanını seçin

### 5. Grafik Oluşturma
- "Grafik Oluştur" butonuna tıklayın
- Dinamik grafik ekranda görüntülenir

## 🔐 Test Kullanıcı Bilgileri

### Login Bilgileri
```
Username: admin
Password: Aa123456.
```

## 📊 API Endpoints

### Veritabanı Endpoints
```
POST /api/inspect/views
POST /api/inspect/procedures  
POST /api/inspect/functions
```

### Veri Çalıştırma Endpoint
```
POST /api/execute-object
```

## 🔧 Geliştirme Ortamı

### Visual Studio 2022 Kurulumu
1. Visual Studio 2022 Community/Professional yükleyin
2. ASP.NET Core workload'unu seçin
3. Projeyi Visual Studio'da açın
4. F5 ile debug modda çalıştırın

### Debugging
- Browser Developer Tools'u açın (F12)
- Console sekmesinden JavaScript hatalarını takip edin
- Network sekmesinden API çağrılarını kontrol edin

## 🔒 Güvenlik

### JWT Authentication
- API endpoints JWT token ile korunmaktadır
- Token süresi: 30 dakika (konfigürasyondan değiştirilebilir)

  ## Görseller
 - <img width="259" height="193" alt="image" src="https://github.com/user-attachments/assets/3d78721f-b0db-4f26-919c-b246d6249817" />

 - <img width="471" height="400" alt="image" src="https://github.com/user-attachments/assets/8a2cca58-f7ff-4aa3-9fc2-12766537b68d" />

 - <img width="1379" height="628" alt="image" src="https://github.com/user-attachments/assets/58f3448f-236b-4ea5-9265-b0bace02e2e4" />

 - <img width="1332" height="433" alt="image" src="https://github.com/user-attachments/assets/2b64c11d-098b-44a6-932c-7e5a531fa9e4" />

 - <img width="1313" height="652" alt="image" src="https://github.com/user-attachments/assets/9e728d99-6f5a-4afd-a7c5-7ebce0f79181" />

 - <img width="1336" height="239" alt="image" src="https://github.com/user-attachments/assets/aee53820-993f-4ab3-ade8-b8de4c4540ef" />

 - <img width="1321" height="801" alt="image" src="https://github.com/user-attachments/assets/3aa9db80-dc98-40f4-b0f7-fd9295b87f94" />






