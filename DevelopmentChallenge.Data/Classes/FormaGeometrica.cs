using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DevelopmentChallenge.Data.Classes
{
    public interface IShape
    {
        decimal CalculateArea();
        decimal CalculatePerimeter();
        string GetName(ILanguageStrategy language, int count);
    }

    public abstract class Shape : IShape
    {
        protected readonly decimal side;

        protected Shape(decimal side)
        {
            this.side = side;
        }

        public abstract decimal CalculateArea();
        public abstract decimal CalculatePerimeter();
        public abstract string GetName(ILanguageStrategy language, int count);
    }

    public class Square : Shape
    {
        public Square(decimal side) : base(side) { }

        public override decimal CalculateArea()
        {
            return side * side;
        }

        public override decimal CalculatePerimeter()
        {
            return side * 4;
        }

        public override string GetName(ILanguageStrategy language, int count)
        {
            return language.GetSquareName(count);
        }
    }

    public class Circle : Shape
    {
        public Circle(decimal radius) : base(radius) { }

        public override decimal CalculateArea()
        {
            return (decimal)Math.PI * (side / 2) * (side / 2);
        }

        public override decimal CalculatePerimeter()
        {
            return (decimal)Math.PI * side;
        }

        public override string GetName(ILanguageStrategy language, int count)
        {
            return language.GetCircleName(count);
        }
    }

    public class EquilateralTriangle : Shape
    {
        public EquilateralTriangle(decimal side) : base(side) { }

        public override decimal CalculateArea()
        {
            return ((decimal)Math.Sqrt(3) / 4) * side * side;
        }

        public override decimal CalculatePerimeter()
        {
            return side * 3;
        }

        public override string GetName(ILanguageStrategy language, int count)
        {
            return language.GetTriangleName(count);
        }
    }

    public class Trapezoid : Shape
    {
        private readonly decimal height;
        private readonly decimal topSide;
        private readonly decimal bottomSide;

        public Trapezoid(decimal height, decimal topSide, decimal bottomSide, decimal side) : base(side)
        {
            this.height = height;
            this.topSide = topSide;
            this.bottomSide = bottomSide;
        }

        public override decimal CalculateArea()
        {
            return height * (topSide + bottomSide) / 2;
        }

        public override decimal CalculatePerimeter()
        {
            return topSide + bottomSide + (2 * side);
        }

        public override string GetName(ILanguageStrategy language, int count)
        {
            return language.GetTrapezoidName(count);
        }
    }

    public interface ILanguageStrategy
    {
        string GetReportHeader();
        string GetEmptyListMessage();
        string GetSquareName(int count);
        string GetCircleName(int count);
        string GetTriangleName(int count);
        string GetTrapezoidName(int count);
        string GetShapesWord(int count);
        string GetPerimeterWord();
        string GetAreaWord();
    }

    public class SpanishLanguageStrategy : ILanguageStrategy
    {
        public string GetReportHeader() { return "Reporte de Formas"; }
        public string GetEmptyListMessage() { return "Lista vacía de formas!"; }
        public string GetSquareName(int count) { return count == 1 ? "Cuadrado" : "Cuadrados"; }
        public string GetCircleName(int count) { return count == 1 ? "Círculo" : "Círculos"; }
        public string GetTriangleName(int count) { return count == 1 ? "Triángulo" : "Triángulos"; }
        public string GetTrapezoidName(int count) { return count == 1 ? "Trapecio" : "Trapecios"; }
        public string GetShapesWord(int count) { return "formas"; }
        public string GetPerimeterWord() { return "Perimetro"; }
        public string GetAreaWord() { return "Area"; }
    }

    public class EnglishLanguageStrategy : ILanguageStrategy
    {
        public string GetReportHeader() { return "Shapes report"; }
        public string GetEmptyListMessage() { return "Empty list of shapes!"; }
        public string GetSquareName(int count) { return count == 1 ? "Square" : "Squares"; }
        public string GetCircleName(int count) { return count == 1 ? "Circle" : "Circles"; }
        public string GetTriangleName(int count) { return count == 1 ? "Triangle" : "Triangles"; }
        public string GetTrapezoidName(int count) { return count == 1 ? "Trapezoid" : "Trapezoids"; }
        public string GetShapesWord(int count) { return "shapes"; }
        public string GetPerimeterWord() { return "Perimeter"; }
        public string GetAreaWord() { return "Area"; }
    }

    public class ShapeReportGenerator
    {
        private readonly ILanguageStrategy language;

        public ShapeReportGenerator(ILanguageStrategy language)
        {
            this.language = language;
        }

        public string GenerateReport(List<IShape> shapes)
        {
            if (!shapes.Any())
            {
                return $"<h1>{language.GetEmptyListMessage()}</h1>";
            }

            var sb = new StringBuilder();
            sb.Append($"<h1>{language.GetReportHeader()}</h1>");

            var shapeGroups = shapes.GroupBy(s => s.GetType())
                                  .Select(g => new
                                  {
                                      Count = g.Count(),
                                      Area = g.Sum(s => s.CalculateArea()),
                                      Perimeter = g.Sum(s => s.CalculatePerimeter()),
                                      Type = g.First()
                                  });

            var culture = language is EnglishLanguageStrategy
                ? CultureInfo.InvariantCulture
                : CultureInfo.GetCultureInfo("es-ES");

            foreach (var group in shapeGroups)
            {
                sb.Append($"{group.Count} {group.Type.GetName(language, group.Count)} | ");
                sb.Append($"{language.GetAreaWord()} {group.Area.ToString("#.##", culture)} | ");
                sb.Append($"{language.GetPerimeterWord()} {group.Perimeter.ToString("#.##", culture)} <br/>");
            }


            // Add footer
            sb.Append("TOTAL:<br/>");
            int totalShapes = shapes.Count;
            decimal totalPerimeter = shapes.Sum(s => s.CalculatePerimeter());
            decimal totalArea = shapes.Sum(s => s.CalculateArea());

            sb.Append($"{totalShapes} {language.GetShapesWord(totalShapes)} ");
            sb.Append($"{language.GetPerimeterWord()} {totalPerimeter.ToString("#.##", culture)} ");
            sb.Append($"{language.GetAreaWord()} {totalArea.ToString("#.##", culture)}");

            return sb.ToString();
        }
    }

    public static class LanguageFactory
    {
        public const int Spanish = 1;
        public const int English = 2;

        public static ILanguageStrategy GetLanguage(int languageId)
        {
            switch (languageId)
            {
                case Spanish:
                    return new SpanishLanguageStrategy();
                case English:
                    return new EnglishLanguageStrategy();
                default:
                    return new EnglishLanguageStrategy();
            }
        }
    }
}