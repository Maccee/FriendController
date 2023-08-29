using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FriendController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        public static List<string> Friends = new List<string>()
        {
            "Josi,Mario",
            "Paavo,Juha"
        };

        [HttpGet]
        public List<string> Get()
        {
            return Friends;
        }

        [HttpGet("{name}")]
        public string GetByName(string name)
        {

            List<string> kaverit = new List<string>();

            foreach (var listaItem in Friends)
            {
                
                var namesArray = listaItem.Split(',');

                if (namesArray[0] == name) 
                {
                    kaverit.AddRange(namesArray.Skip(1)); 
                }
                else if (namesArray.Contains(name)) 
                {
                    kaverit.Add(namesArray[0]); 
                }
            }

            if (kaverit.Any())
            {
                return string.Join(", ", kaverit.Distinct());
            }

            return $"{name} ei ole yhtään ystävää. :(";
        }


        [HttpGet("{name}/{friend}")]
        public string AddFriend(string name, string friend)
        {
            bool nameExists = false;
            bool friendExistsAsPrimary = false;
            if (!Friends.Any(f => f.Split(',').Contains(name)))
            {
                return $"Ei löydy: {name}";
            }
            for (int i = 0; i < Friends.Count; i++)
            {
                var namesArray = Friends[i].Split(',');
                if (namesArray[0] == name)
                {
                    nameExists = true;
                    if (namesArray.Contains(friend))
                    {
                        return $"{name} ja {friend} ovat jo ystäviä! <3";
                    }
                    else
                    {
                        Friends[i] = Friends[i] + "," + friend;
                    }
                }
                if (namesArray[0] == friend)
                {
                    friendExistsAsPrimary = true;
                }
            }
            if (!nameExists && !friendExistsAsPrimary)
            {
                Friends.Add($"{friend},{name}");
                return $"{friend} ja {name} ovat nyt kavereita!";
            }
            else if (nameExists && !friendExistsAsPrimary)
            {
                Friends.Add($"{friend},{name}");
            }
            return $"{friend} ja {name} ovat nyt kavereita!";
        }

        [HttpDelete("{name}/{friend}")]
        public string RemoveFriendship(string name, string friend)
        {
            bool friendshipFound = false;

            for (int i = 0; i < Friends.Count; i++)
            {
                var namesArray = Friends[i].Split(',');

                if (namesArray[0] == name && namesArray.Contains(friend))
                {
                    Friends[i] = string.Join(",", namesArray.Where(n => n != friend));
                    friendshipFound = true;
                }
                else if (namesArray[0] == friend && namesArray.Contains(name))
                {
                    Friends[i] = string.Join(",", namesArray.Where(n => n != name));
                    friendshipFound = true;
                }
            }

            if (friendshipFound)
            {
                return $"{name} ja {friend} eivät ole enää kavereita. :(";
            }

            return $"{name} ja {friend} eivät olleet ystäviä.";
        }






    }
}