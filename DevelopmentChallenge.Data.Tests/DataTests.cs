using System.Collections.Generic;
using DevelopmentChallenge.Data.Classes;
using NUnit.Framework;

namespace DevelopmentChallenge.Data.Tests
{
    [TestFixture]
    public class DataTests
    {
        private ShapeReportGenerator spanishReportGenerator;
        private ShapeReportGenerator englishReportGenerator;

        [SetUp]
        public void Setup()
        {
            spanishReportGenerator = new ShapeReportGenerator(LanguageFactory.GetLanguage(LanguageFactory.Spanish));
            englishReportGenerator = new ShapeReportGenerator(LanguageFactory.GetLanguage(LanguageFactory.English));
        }

        [Test]
        public void TestEmptyList()
        {
            Assert.That(spanishReportGenerator.GenerateReport(new List<IShape>()), Is.EqualTo("<h1>Lista vacía de formas!</h1>"));
            Assert.That(englishReportGenerator.GenerateReport(new List<IShape>()), Is.EqualTo("<h1>Empty list of shapes!</h1>"));
        }

        [Test]
        public void TestSingleSquare()
        {
            var shapes = new List<IShape> { new Square(5) };
            var result = spanishReportGenerator.GenerateReport(shapes);
            Assert.That(result, Is.EqualTo("<h1>Reporte de Formas</h1>1 Cuadrado | Area 25 | Perimetro 20 <br/>TOTAL:<br/>1 formas Perimetro 20 Area 25"));
        }

        [Test]
        public void TestMultipleSquares()
        {
            var shapes = new List<IShape>
            {
                new Square(5),
                new Square(1),
                new Square(3)
            };
            var result = englishReportGenerator.GenerateReport(shapes);
            Assert.That(result, Is.EqualTo("<h1>Shapes report</h1>3 Squares | Area 35 | Perimeter 36 <br/>TOTAL:<br/>3 shapes Perimeter 36 Area 35"));
        }

        [Test]
        public void TestMultipleShapesInEnglish()
        {
            var shapes = new List<IShape>
            {
                new Square(5),
                new Circle(3),
                new EquilateralTriangle(4),
                new Square(2),
                new EquilateralTriangle(9),
                new Circle(2.75m),
                new EquilateralTriangle(4.2m)
            };
            var result = englishReportGenerator.GenerateReport(shapes);
            Assert.That(result, Is.EqualTo("<h1>Shapes report</h1>2 Squares | Area 29 | Perimeter 28 <br/>2 Circles | Area 13.01 | Perimeter 18.06 <br/>3 Triangles | Area 49.64 | Perimeter 51.6 <br/>TOTAL:<br/>7 shapes Perimeter 97.66 Area 91.65"));
        }

        [Test]
        public void TestMultipleShapesInSpanish()
        {
            var shapes = new List<IShape>
            {
                new Square(5),
                new Circle(3),
                new EquilateralTriangle(4),
                new Square(2),
                new EquilateralTriangle(9),
                new Circle(2.75m),
                new EquilateralTriangle(4.2m)
            };
            var result = spanishReportGenerator.GenerateReport(shapes);
            Assert.That(result, Is.EqualTo("<h1>Reporte de Formas</h1>2 Cuadrados | Area 29 | Perimetro 28 <br/>2 Círculos | Area 13,01 | Perimetro 18,06 <br/>3 Triángulos | Area 49,64 | Perimetro 51,6 <br/>TOTAL:<br/>7 formas Perimetro 97,66 Area 91,65"));
        }

        [Test]
        public void TestTrapezoidCalculations()
        {
            var trapezoid = new Trapezoid(4, 6, 10, 5);
            Assert.That(trapezoid.CalculateArea(), Is.EqualTo(32));
            Assert.That(trapezoid.CalculatePerimeter(), Is.EqualTo(26));
        }

        [Test]
        public void TestMultipleShapesWithTrapezoid()
        {
            var shapes = new List<IShape>
            {
                new Square(5),
                new Trapezoid(4, 6, 10, 5),
                new Circle(3)
            };
            var result = spanishReportGenerator.GenerateReport(shapes);
            Assert.That(result, Is.EqualTo("<h1>Reporte de Formas</h1>1 Cuadrado | Area 25 | Perimetro 20 <br/>1 Trapecio | Area 32 | Perimetro 26 <br/>1 Círculo | Area 7,07 | Perimetro 9,42 <br/>TOTAL:<br/>3 formas Perimetro 55,42 Area 64,07"));
        }
    }
}