﻿using Newtonsoft.Json;

using Pokemon_API.Extensions;

namespace Pokemon_API.DatabaseSchemas.Pokemon.Models
{
    public class GenderRatio : DatabaseTable<GenderRatio>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        [JsonIgnore]
        public int PokemonNumber { get; set; }

        [JsonProperty(PropertyName="male", Order = 1)]
        public float Male { get; set; }

        [JsonProperty(PropertyName="female", Order = 2)]
        public float Female { get; set; }

        public GenderRatio()
        {
        }

        public GenderRatio(int? id, int pokemonNumber, float male, float female)
        {
            this.Id = id;
            this.PokemonNumber = pokemonNumber;
            this.Male = male;
            this.Female = female;
        }
    }
}
