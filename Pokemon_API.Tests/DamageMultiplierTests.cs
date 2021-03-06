﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;

using Pokemon_API;

using Pokemon_API.Extensions;
using Pokemon_API.DatabaseSchemas.DamageMultiplier;
using Pokemon_API.Functions;
using Newtonsoft.Json;

namespace Pokemon_API.Tests
{
    [Collection("Sequential")]
    public class DamageMultiplierTests
    {
        const string SUCCESS = "SUCCESS";

        public DamageMultiplierTests()
        {
        }


        [Fact]
        public async void TestGetMultiplier()
        {
            ResponseModels.MultiplierResponse dmgMult = await (new DatabaseSchemas.DamageMultiplier.Builder().Build("normal"));

            Assert.NotNull(dmgMult);

            var function = new GetDamageMultiplierFunction();
            APIGatewayProxyRequest request = new APIGatewayProxyRequest();
            request.PathParameters = new Dictionary<string, string>() { { "type1", "normal" } };
            var context = new TestLambdaContext();
            var response = await function.Execute(request, context);

            dmgMult = JsonConvert.DeserializeObject<ResponseModels.MultiplierResponse>(response.Body);
            Assert.NotNull(dmgMult);
        }

        [Fact]
        public async void Test_DamageMultiplier_Multiplier()
        {
            Console.WriteLine("-- | Damage Multipliers | Multipliers | --");

            var table = new DatabaseSchemas.DamageMultiplier.Tables.Multiplier();

            Console.WriteLine("-- Testing Insert --");

            var data = new DatabaseSchemas.DamageMultiplier.Models.Multiplier(
                id: null,
                name: "test",
                normal: 10,
                fighting: 10,
                flying: 10,
                poison: 10,
                ground: 10,
                rock: 10,
                bug: 10,
                ghost: 10,
                steel: 10,
                fire: 10,
                water: 10,
                grass: 10,
                electric: 10,
                psychic: 10,
                ice: 10,
                dragon: 10,
                dark: 10,
                fairy: 10
            );

            await table.Delete(data.Name);

            int? result = await table.Insert(data);
            Assert.NotNull(result);

            Console.WriteLine(SUCCESS);
            Console.WriteLine("-- Testing Get --");

            DatabaseSchemas.DamageMultiplier.Models.Multiplier get = await table.Get(data.Name);

            Assert.NotNull(get.Id);
            data.Id = get.Id;
            Assert.Equal(data, get);

            Console.WriteLine(SUCCESS);
            Console.WriteLine("-- Testing Delete --");

            result = await table.Delete(data.Name);
            Assert.NotNull(result);

            get = await table.Get(get.Name);
            Assert.Null(get);

            Console.WriteLine(SUCCESS);
        }
    }
}
