using System.Collections;
using System.Collections.Generic;

namespace BookAPIProject.Models.Services
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries();
        Country GetCountry(int ContryId);
        Country GetCountryOfAuthor(int AuthorId);
        ICollection<Author> GetAuthorsFromContry(int ContryId);
        bool CountryExist(int ContryId);
        bool IDuplicateCountryName(int CountryId, string CountryName);
        bool Add(Country country);
        bool Delete(Country country);
        bool Update(Country country);
        bool Save();
    }
}
