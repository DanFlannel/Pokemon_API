﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

using Newtonsoft.Json;

using Pokemon_API.Extensions;
using Pokemon_API.ResponseModels;
using Pokemon_API.DatabaseSchemas.Pokemon;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace Pokemon_API.Functions
{
    public class GetPokemonFunction
    {
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public GetPokemonFunction()
        {
        }

        public async Task<APIGatewayProxyResponse> Execute(APIGatewayProxyRequest request, ILambdaContext context)
        {
            _ = (request.PathParameters.TryGetValue("id", out string id));
            _ = (request.PathParameters.TryGetValue("level", out string level));

            if (string.IsNullOrEmpty(id))
            {
                return APIGatewayProxyResponseExtensions.Fail($"Please enter pokemon name or number");
            }

            id = Uri.UnescapeDataString(id);
            level = (!string.IsNullOrEmpty(level)) ? Uri.UnescapeDataString(level) : level;

            try
            {
                PokemonResponse jsonResponse = await GetResponse(id, level);
                if (jsonResponse == null)
                {
                    return APIGatewayProxyResponseExtensions.Fail($"Pokemon: {id} not found");
                }
                return APIGatewayProxyResponseExtensions.Sucess(JsonConvert.SerializeObject(jsonResponse));
            }
            catch (Exception e)
            {
                context.Logger.Log(e.Message);
                return APIGatewayProxyResponseExtensions.Fail(e.Message);
            }
        }

        public async Task<PokemonResponse> GetResponse(string id, string level)
        {
            PokemonResponse response = await new Builder().Build(id);
            //do level adjustment logic here
            return response;
        }
    }
}
