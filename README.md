# Saga Patterns

## 1- Saga pattern mikroservis mimarisinde hangi sorunları çözmeye çalışır?

  Mikroservis mimarisinde sistem her biri kendi iş bölümünden ve veri yönetiminden sorumlu küçük servislere bölünmüş durumdadır ve sistemde birden fazla mikroservisin katıldığı bir işlemin gerçekleşebilmesi için gerekli mikroservislerin koordine bir şekilde çalışması ve herhangi bir serviste meydana gelecek olası bir hata senaryosunun yönetilerek sistemin ve verilerin tutarlılığının sağlanması gereklidir. Mikroservislerin kendi veri tabanları üzerinde işlem yaptığını düşünürsek birden fazla mikroservisin kullanıldığı işlemlerde herhangi bir adımda hata meydana gelmesi durumunda önceki adımlarda güncellenen verilerin sonraki adımlarda güncellenememesi veri tutarsızlığına yol açacaktır. Böyle bir durumda gerekli telafi ve geri alma işlemlerinin gerçekleştirilip veri tutarlılığının sağlanması gereklidir. Saga Pattern, mikroservisler arasında işlem bütünlüğü ve veri tutarlığını sağlamayı hedefleyen bir tasarım desenidir. Saga pattern'a göre bir sonraki adıma geçmek için bir önceki adımın başarılı olması gerekir, eğer bir adımda hata alınırsa sistem o adıma gelene kadar gerçekleşen tüm aşamalar için gerekli rollback işlemlerini yürüterek sistemi ilk andaki tutarlı durumuna geri getirmeyi amaçlar.

## 2- Saga patterndeki choreography ve orchestration yaklaşımları arasındaki temel fark nedir?

Choreography ve Orchestration yaklaşımları arasındaki temel fark işleyiş biçimidir.
 
Orchestration yaklaşımında tüm süreç merkezi bir orchestrator tarafından yönetilir. Orhestrator hangi işlemlerin hangi sırayla çalışacağını kontrol eder ve olası bir hata durumunda gerekli telafi işlemlerini çağırır.

Choreography yaklaşımında ise bir mikroservis kendi lokal işlemini tamamladıktan ya da olası hata durumlarından sonra diğer mikroservislerin okuyup aksiyon alması için gerekli bilgilerin iletildiği eventler oluşturur. Choreography yaklaşımında mikroservisler arada merkezi bir denetleyici olmadan birbirleriyle direkt iletişim kurarlar.

## 3- Orchestration Saga pattern avantajları ve dezavantajları nelerdir? 

### Avantajlar:

- Orchestration yaklaşımında tüm süreç merkezi bir yapı tarafından kontrol edildiği için izlemeyi ve hata takibi yapmayı daha kolay bir hale getirir.

- Orchestrator yönettiği servislere bağlı durumda olurken bu servislerin birbirlerine olan bağlılığını ortadan kaldırır.

- Yeni adımlar eklemek ve test etmek kolaydır.

- İş akışı orchestrator implement edilirken tanımlandığı için iş akışını anlamak ve oluşabilecek çıktıları tahmin etmek kolaydır.

### Dezavantajlar:

- Fazladan bir servis(Orchestrator) implement etmek gereklidir ve altyapı karmaşıklığını arttırır.
	
- Tek bir yönetim noktası olması bu noktanın arızalanması halinde tüm sürecin aksamasına yol açabilir.

## 4.1- 

![saga_state_machine](https://github.com/user-attachments/assets/403e9f16-7954-4071-a7ea-c38b79cbff71)

## 4.2- Her bir durumda, ilgili hizmetin başarılı ya da başarısız olması durumunda nasıl bir geçiş yapılacağını açıklayın. 

### Stok Kontrolü: 

Sipariş edilen ürünlerin stok kontrolü yapılır. Stokta yeteri kadar ürün varsa başarılı durum, yoksa başarısız durum tetiklenir.
* Başarılı durum: 
Stoktaki ürünler rezerve edilir ve ödeme adımına geçilir.
* Başarısız durum: 
Stokta yeteri kadar ürün olmadığı için rezerve işlemi yapılmaz ve sipariş iptal edilir.

### Ödeme İşlemi: 
Müşterinin bakiyesi kontrol edilir eğer yeterli bakiye varsa başarılı durum, yoksa başarısız durum tetiklenir.  
* Başarılı Durum: 
Müşteriden gerekli miktarda ödeme alınır ve kargo adımına geçilir.		
* Başarısız Durum: 
Ödeme başarısız olur, stokta rezerve edilen ürünler serbest bırakılır ve sipariş iptal edilir.

### Kargo İşlemi: 

Sipariş müşteriye teslim edilmek üzere hazırlanır ve gönderilir, kargo müşteriye teslim edilirse başarılı durum, teslim edilemezse başarısız durum tetiklenir.
* Başarılı Durum: 
Kargo müşteriye teslim edilir ve sipariş tamamlanır.
* Başarısız Durum: 
Kargo teslim edilemediği için müşteriye ödeme iadesi yapılır, sonrasında stokta rezerve edilen ürünler serbest bırakılır, son olarak sipariş iptal edilir.

# Unit Test 

## 1- .NET platformunda unit test yapma sürecini açıklayın ve örnek olarak Xunit ve Moq kütüphanelerini kullanarak bir unit test yazın. 

.NET platformunda unit test yapmak için öncelikle bir unit test framework'ü seçilmelidir. .NET' deki en popüler unit test frameworkleri xUnit, NUnit ve MSTest'dir. Bir unit test framework'ü seçildikten sonra Visual Studio üzerinden test edilmek istenilen projenin bulunduğu solution içerisine o framework'e uygun olan proje şablonu (Örneğin:xUnit Test Project) seçilerek bir test projesi oluşturulur ve test edilecek olan ana projenin bağımlılıkları test projesine eklenir. Eğer test edilecek birimin dış bağımlılıkları varsa bu bağımlılıkları izole etmek için gerekli Mock işlemleri seçilen bir mocking kütüphanesi(Örneğin:Moq) aracılığıyla yapılır. Sonrasında test metodları yazılır. Test metodları genellikle 3 adımdan oluşur: Arrange, Act ve Assert. Arrange adımında test edilecek birim için gerekli hazırlıklar yapılır(parametreleri belirleme, gerekli nesneleri oluşturma vs.), Act adımında test edilmek istenen metod çağrılır, Assert adımında ise elde edilen sonuçların beklenen sonuçlar ile karşılaştırılması yapılır. Test metodları yazıldıktan sonra test projesi çalıştırılır ve testlerden elde edilen sonuçlar değerlendirilir.


## 2- Xunit ve Moq Temel Kavramları:

## 2.1- Mocked object üretme: Testlerde bağımlılıkları izole etmek için nesneleri nasıl "mock"layabiliriz? 

Bağımlılıkların testlerde izole edilmesi, birim testlerinin doğru ve bağımsız çalışabilmesi için önemlidir. İzole edilmesi gereken bağımlılıkları taklit etmek (mocking) için Moq kütüphanesi kullanılır. Mock nesneler, testleri dış bağımlılıklardan bağımsız hale getirir.

### Moq kütüphanesinde sıkça kullanılan metodlar:

- `Mock<T>` sınıfı, verilen bir arayüz ya da sınıfın taklit(mock) nesnesini oluşturur. Burada T taklit edilecek sınıf ya da arayüzdür.
Örneğin:
`var entityRepositoryMock = new Mock<IEntityRepository>();` 
 kod satırı ile IEntityRepository arayüzünden bir mock oluşturarak testimizi bu arayüzün bağımlılığından izole etmek için oluşturduğumuz mock'u kullanabiliriz.

- `Setup()` Metodu: Mock nesnesinin metodlarının nasıl çalışacağını belirlemek içi Setup() metodunu kullanırız.
Örneğin:
`entityRepositoryMock.Setup(repository => repository.GetAll()).Returns(new List<Person>());`
 oluşturduğumuz mock nesnenin GetAll() metodu çağrıldığında geriye Person sınıfından nesneler tutan boş bir liste dönmesini sağlarız.

- `Returns()` Metodu: Mock nesnesin çağırdığı metodun döndüreceği değeri belirlemek için Returns() metodu kullanılır.

- `Throws()` Metodu: Mock nesnesinin çağırdığı metodun hata fırlatmasını sağlamak için kullanılır.

- `Callback()` Metodu: Mock nesnesinin çağırdığı metod çağrıldığında bir callback metodu çalıştırmak için kullanılır.

- `It.IsAny<T>()` Metodu: Mock nesnesinin çağırdığı metoda geçilecek olan parametrenin doğru türde olması koşuluyla herhangi bir değer için geçerli olmasını sağlar.

- `Verify()` Metodu: Mock nesnesi üzerinden çağrılan metodların doğrulamasını yapmak için kullanılır. Metodların belirli bir sayıda çağrılıp çağrılmadığını doğrular.
Örneğin: 
`entityRepositoryMock.Verify(repository => repository.GetAll(), Times.Once);`
`GetAll()` metodunun yalnızca bir kez çağrıldığını doğrular.

## 2.2- Assert İşlemleri
Bir testin temel amacı, belirli bir işlemin doğru şekilde çalışıp çalışmadığını kontrol etmektir. Bu doğrulama için XUnit'de Assert sınıfı kullanılır. Assert işlemleri, test sonucundan beklenen değer ile test sonucunda elde edilen gerçek değerler arasındaki farkları kontrol etmek için kullanılır.

### Sıkça Kullanılan Assert Fonksiyonları:
- `Assert.Equal(expected, actual)`: Beklenen değerin(expected) gerçek değer(actual) ile eşit olup olmadığını doğrular.

- `Assert.IsType<expectedType>(object)`: Parametre olarak girilen değerin beklenen tipte(expectedType) olduğunu doğrular.
 
- `Assert.Null(value)`: Value değerinin null olduğunu doğrular.

- `Assert.NotNull(value)`: Value değerinin null olmadığını doğrular.

- `Assert.True(condition)`: Verilen koşulun 'true' olduğunu doğrular.

- `Assert.False(condition)`: Verilen koşulun 'false' olduğunu doğrular.

## 2.3- Fact ve Theory
XUnit'te Fact parametresiz testler için, Theory ise parametreli testler için kullanılır. Theory ile birden fazla veri seti test edilebilir.

### Fact:
Fact, bir metodun parametresiz bir test metodu olduğunu işaretlemek ve bir test senaryosunu test etmek için kullanılır. Test gövdesi belirlenen sabit değerler ile test edilir.

### Theory:
Theory, bir metodun parametrelendirilmiş bir test metodu olduğunu işaretlemek için kullanılır. Test gövdesinin farklı girdi değerleri ile birden fazla kez çalışmasını sağlar. Girdi verileri, InlineData, ClassData, MemberData gibi attributelar ile verilebilir.

### Fact ve Theory Kullanımının Kod Üzerinde Uygulanması
#### Test edilecek Fonksiyon
 ![Ekran görüntüsü 2024-11-22 111121](https://github.com/user-attachments/assets/aae78db7-e52e-4bf0-95ac-cc7dfbf12806)
#### Fact ve Theory kullanarak Test metodlarının yazılması
 ![Ekran görüntüsü 2024-11-22 111133](https://github.com/user-attachments/assets/f054c429-28ec-45e8-88e5-5939992c6f8f)
#### Test Sonuçları
![Ekran görüntüsü 2024-11-22 111328](https://github.com/user-attachments/assets/cdfeb02b-d2e6-4a02-a291-4aad3158e09e)
Kaynak kodlarına https://github.com/nurbakibozkurt/FactAndTheoryPractice adresinden erişebilirsiniz.

## 3- Repository Sınıfları Üzerinde Pratik
### 3.1- Repository sınıfları için bir CRUD yapısı kurarak testler yazın. Bu süreçte Entity Framework Core kullanarak basit bir CRUD işlemi hazırlayın.
Bir ASP.Net Web Api projesinde Entity Framework Core kullanılarak CRUD işlemleri yapılmış, daha sonrasında hem Repository hem de Controller sınıfları için xUnit ve Moq kütüphaneleri kullanılarak unit testler gerçekleştirilmiştir.
Projenin dosyalarına bu github repository'si üzerinden erişebilirsiniz.
![Ekran görüntüsü 2024-11-22 120731](https://github.com/user-attachments/assets/cea6852e-a696-4b3f-9d0f-666a6fcab0f6)
Yapılan Unit Testlerin Sonuçları

### 3.2- Mapper’ı ve veritabanını mocklama işlemlerini nasıl yapacağınızı gösterin. 
#### Mapper'ı mocklama işlemi
![Ekran görüntüsü 2024-11-22 113711](https://github.com/user-attachments/assets/39b05877-0901-42ae-bf33-d21fd595e196)
![Ekran görüntüsü 2024-11-22 113745](https://github.com/user-attachments/assets/c701155a-a7c7-4af5-94d5-b5ee698092ad)
- `_mapperMock = new Mock<IMapper>();` satırı ile mapper mock'u oluşturuluyor.
- 115. satırdaki `_mapperMock.Setup(mapper => mapper.Map<Person>(personToBeAddedDto)).Returns(personToBeAdded);` kodu ile Mocklanan Mapper nesnesinin `Map()` metodunun kendisine verilen `PersonDto` sınıfından bir nesneyi `Person` sınıfından bir nesneye map etmesini sağlıyoruz.
#### Veritabanını mocklama işlemi

![Ekran görüntüsü 2024-11-22 115016](https://github.com/user-attachments/assets/7a288b8b-e635-453f-94be-43cb5de12129)
- `_appDbcontextMock = new Mock<AppDbContext>();` satırı ile dbcontext mock'u oluşturuluyor.
- `_mockData` listesinde test için kullanılacak taklit veritabanı kayıtları tutuluyor ve bu liste GetMock metoduna parametre olarak geçiliyor.
- GetMock metodunda `var mockDbSet = new Mock<DbSet<Person>>();` satırı ile dbset mock'u oluşturuluyor ve bu dbset'e _mockData içindeki kayıtlar geçiliyor.
- `_appDbcontextMock.Setup(x => x.Set<Person>()).Returns(mockDbSet.Object);` satırı ile mocklanan dbcontext'in `Set<Person>()` metodunun dbset Mock'unu döndürmesi sağlanıyor.
