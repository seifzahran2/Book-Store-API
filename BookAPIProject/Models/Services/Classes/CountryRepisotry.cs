using System.Collections.Generic;
using System.Linq;

namespace BookAPIProject.Models.Services
{
    public class CountryRepositry : ICountryRepository
    {
        private readonly BookDbContext _BookDbContext;
        public CountryRepositry(BookDbContext BookDbContext)
        {
            _BookDbContext = BookDbContext;
        }

        public bool CountryExist(int ContryId)
        {
            return _BookDbContext.Countries.Any(c=>c.Id == ContryId);
        }

        public ICollection<Author> GetAuthorsFromContry(int ContryId)
        {
            return _BookDbContext.Author.Where(c=>c.Country.Id == ContryId).ToList();
        }

        public ICollection<Country> GetCountries()
        {
            return _BookDbContext.Countries.OrderBy(c => c.Name).ToList();
        }

        public Country GetCountry(int ContryId)
        {
            return _BookDbContext.Countries.Where(c => c.Id == ContryId).FirstOrDefault();
        }

        public Country GetCountryOfAuthor(int AuthorId)
        {
            return _BookDbContext.Author.Where(a => a.Id == AuthorId)
                .Select(c => c.Country).FirstOrDefault();
        }

        public bool IDuplicateCountryName(int CountryId, string CountryName)
        {
            var country = _BookDbContext.Countries.Where(c => c.Name.Trim().ToUpper() == CountryName.Trim().ToUpper() && c.Id != CountryId).FirstOrDefault();
            return country == null ? false : true;
        }

        public bool Add(Country country)
        {
            _BookDbContext.Add(country);
            return Save();
        }

        public bool Update(Country country)
        {
            _BookDbContext.Update(country);
            return Save();
        }

        public bool Delete(Country country)
        {
            _BookDbContext.Remove(country);
            return Save();
        }

        public bool Save()
        {
            var saved = _BookDbContext.SaveChanges();
            return saved >= 0? true: false;
        }
    }
}
