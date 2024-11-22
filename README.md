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
