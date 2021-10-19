  var responseData = ElasticSeachConfiguration.ESContext.Search<Patient>(s => s
                .From(pageFrom)
                .Size(pageSize)
                .Index(elasticPatientSearchIndex)
                .Query(q => 
                      +q.QueryString(mm => mm
                        .Query(term)
                        .DefaultOperator(Operator.And)
                        .Fields(f => f
                                    .Field(d => d.Name)
                                    .Field(d => d.LastName)
                                    .Field(d => d.PhoneNumbers.First().Phone.First())  ///for nested fields 
                                    .Field(d => d.IdentityNumber))
)));
