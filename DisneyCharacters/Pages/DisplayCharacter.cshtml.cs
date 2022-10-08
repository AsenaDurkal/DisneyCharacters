using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApplication2.Pages
{

    public class InitializationException : Exception

    {

    }

   
    public class  Character
    {
        [JsonPropertyName("_id")]
        public int Id { get; set; }

        [JsonPropertyName("films")]
        public IList<string> Films { get; set; }

        [JsonPropertyName("shortFilms")]
        public IList<string> ShortFilms { get; set; }

        [JsonPropertyName("tvShows")]

        public IList<string> TvShows { get; set; }

        [JsonPropertyName("videoGames")]

        public IList<string> VideoGames { get; set; }

        [JsonPropertyName("parkAttractions")]

        public IList<string> ParkAttractions { get; set; }

        [JsonPropertyName("allies")]

        public IList<string> Allies { get; set; }

        [JsonPropertyName("enemies")]

        public IList<string> Enemies { get; set; }

        [JsonPropertyName("name")]

        public string Name { get; set; }

        [JsonPropertyName("imageUrl")]

        public string ImageUrl { get; set; }

        [JsonPropertyName("url")]

        public string Url { get; set; }

    }

   


    public interface ICharProvider
    {
        Task<Character> GetCharacter(int CharCode);


    }

    public class DisneyCharacterProvider: ICharProvider
        {
            public async Task<Character> GetCharacter(int CharCode)
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var streamTask = client.GetStreamAsync("https://api.disneyapi.dev/characters/" +CharCode);
                var streamResult = await streamTask;
                var character = await JsonSerializer.DeserializeAsync<Character>(streamResult);
                return character;

            }
        }

    
    public class DisplayCharacterModel : PageModel
    {
        
        public Character Character { get; set; } = new Character();

        public async Task OnGet(int CharCode)
        {

            ICharProvider provider = new DisneyCharacterProvider();

            Character = await provider.GetCharacter(CharCode);
        }
    }
}
