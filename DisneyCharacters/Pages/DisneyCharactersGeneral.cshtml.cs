using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApplication2.Pages
{

    public class Data
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

        [JsonPropertyName("sourceUrl")]

        public string SourceUrl { get; set; }

        [JsonPropertyName("name")]

        public string Name { get; set; }

        [JsonPropertyName("imageUrl")]

        public string ImageUrl { get; set; }

        [JsonPropertyName("createdAt")]

        public string CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]

        public string UpdatedAt { get; set; }

        [JsonPropertyName("url")]

        public string Url { get; set; }

        [JsonPropertyName("__v")]

        public int __V { get; set; }
    }
    public class DisneyCharacter
    {
        [JsonPropertyName("data")]
        public IList<Data> Datas { get; set; } = new List<Data>();

        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("previousPage")]
        public string PreviousPage { get; set; }

        [JsonPropertyName("nextPage")]
        public string NextPage { get; set; }

    }


    public interface Provider
    {
        Task<DisneyCharacter> GetDisneyCharacters(int PageNumber);


    }



    public class DisneyCharactersProvider : Provider
    {
        public async Task<DisneyCharacter> GetDisneyCharacters(int PageNumber)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            if (PageNumber == 0)
            {
                var streamTask = client.GetStreamAsync("https://api.disneyapi.dev/characters");
                var streamResult = await streamTask;
                var disneyCharacters = await JsonSerializer.DeserializeAsync<DisneyCharacter>(streamResult);
                return disneyCharacters;
            }
            else
            {
                var streamTask = client.GetStreamAsync("https://api.disneyapi.dev/characters?page=" + PageNumber);
                var streamResult = await streamTask;
                var disneyCharacters = await JsonSerializer.DeserializeAsync<DisneyCharacter>(streamResult);
                return disneyCharacters;
            }
        }
        public async Task<DisneyCharacter> GetDisneyCharactersAsc(int PageNumber)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            if (PageNumber == 0)
            {
                var streamTask = client.GetStreamAsync("https://api.disneyapi.dev/characters");
                var streamResult = await streamTask;
                var disneyCharacters = await JsonSerializer.DeserializeAsync<DisneyCharacter>(streamResult);
                List<Data> data = disneyCharacters.Datas.ToList();
                data.Sort((x, y) => string.Compare(x.Name, y.Name));
                disneyCharacters.Datas = data;
                return disneyCharacters;
            }
            else
            {
                var streamTask = client.GetStreamAsync("https://api.disneyapi.dev/characters?page=" + PageNumber);
                var streamResult = await streamTask;
                var disneyCharacters = await JsonSerializer.DeserializeAsync<DisneyCharacter>(streamResult);
                List<Data> data = disneyCharacters.Datas.ToList();
                data.Sort((x, y) => string.Compare(x.Name, y.Name));
                disneyCharacters.Datas = data;
                return disneyCharacters;
            }

        }

        public async Task<DisneyCharacter> GetDisneyCharactersDesc(int PageNumber)

        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            if (PageNumber == 0)
            {
                var streamTask = client.GetStreamAsync("https://api.disneyapi.dev/characters");
                var streamResult = await streamTask;
                var disneyCharacters = await JsonSerializer.DeserializeAsync<DisneyCharacter>(streamResult);
                List<Data> data = disneyCharacters.Datas.ToList();
                data.Sort((x, y) => y.Name.CompareTo(x.Name));
                disneyCharacters.Datas = data;
                return disneyCharacters;
            }
            else
            {
                var streamTask = client.GetStreamAsync("https://api.disneyapi.dev/characters?page=" + PageNumber);
                var streamResult = await streamTask;
                var disneyCharacters = await JsonSerializer.DeserializeAsync<DisneyCharacter>(streamResult);
                List<Data> data = disneyCharacters.Datas.ToList();
                data.Sort((x, y) => y.Name.CompareTo(x.Name));
                disneyCharacters.Datas = data;
                return disneyCharacters;
            }
        }

		
	}

    public class DisneyCharactersGeneralModel : PageModel
    {


        [BindProperty]
        public int CharCode { get; set; }

        [BindProperty]
        public int SortValue { get; set; }

        [BindProperty]

        public int PageNumber { get; set; } 


    


        public DisneyCharacter DisneyCharacter { get; set; } = new DisneyCharacter();
        public async Task OnGet(int? SortValue,int PageNumber)

        {
           DisneyCharactersGeneralModel model = new DisneyCharactersGeneralModel();

            

            DisneyCharactersProvider provider = new DisneyCharactersProvider();


            if (PageNumber == 0)
            {
                if (SortValue == null)
                {

                    DisneyCharacter = await provider.GetDisneyCharacters(PageNumber);
                }
                else if (SortValue == 0)
                {
                    DisneyCharacter = await provider.GetDisneyCharactersAsc(PageNumber);

                }

                else if (SortValue == 1)
                {

                    DisneyCharacter = await provider.GetDisneyCharactersDesc(PageNumber);
                }
            }
            else
            {
                if (SortValue == null)
                {

                    DisneyCharacter = await provider.GetDisneyCharacters(PageNumber);
                }
                else if (SortValue == 0)
                {
                    DisneyCharacter = await provider.GetDisneyCharactersAsc(PageNumber);
                }

                else if (SortValue == 1)
                {

                    DisneyCharacter = await provider.GetDisneyCharactersDesc(PageNumber);
                }

            }


        }


        public IActionResult ActionList()
        {

            return RedirectToPage("./DisplayCharacter", new { charCode = CharCode });
        }


        public IActionResult ActionResult(int SortValue)
        {
           
            return RedirectToPage("./DisneyCharactersGeneral", new { sortValue = SortValue });
        }


        public IActionResult ActionPage(int PageNumber)
        {
            return RedirectToPage("./DisneyCharacterGeneral", new { pageNumber = PageNumber });
        }


    }
}
