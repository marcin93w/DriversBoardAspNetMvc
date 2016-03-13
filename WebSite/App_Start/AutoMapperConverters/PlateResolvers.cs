using AutoMapper;
using Driver.WebSite.Models;

namespace Driver.WebSite.AutoMapperConverters
{
    public class PlateResolver : ValueResolver<Models.Driver, string>
    {
        protected override string ResolveCore(Models.Driver source)
        {
            return PlateResolverHelper.FormatPlateDisplay(source.Plate);
        }
    }

    public class DriverOccurrencePlateResolver : ValueResolver<DriverOccurrence, string>
    {
        protected override string ResolveCore(DriverOccurrence source)
        {
            return PlateResolverHelper.FormatPlateDisplay(source.Driver.Plate);
        }
    }

    internal static class PlateResolverHelper
    {
        public static string FormatPlateDisplay(string plate)
        {
            plate = plate.Replace(" ", string.Empty);
            var indexToInsertSpace = char.IsLetter(plate[2]) ? 3 : 2;
            return plate.Insert(indexToInsertSpace, " ");
        }
    }
}
