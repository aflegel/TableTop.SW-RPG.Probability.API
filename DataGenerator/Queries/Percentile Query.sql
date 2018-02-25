SELECT
/*[PoolCombination].[PositivePoolId],
[PoolCombination].[NegativePoolId],*/
 	   [PositivePool].[Name] + ', ' + [NegativePool].[Name] as [Combination Name]
      ,[PositivePool].[TotalOutcomes] * [NegativePool].[TotalOutcomes] as [Total Outcomes]
      ,[PositivePool].[UniqueOutcomes] * [NegativePool].[UniqueOutcomes] as [Unique Outcomes]

	  ,[SuccessOutcomes]
	  ,(CAST([SuccessOutcomes] as decimal(36,2)) / ([PositivePool].[TotalOutcomes] * [NegativePool].[TotalOutcomes])) AS [Success Rate]
	  ,(([PositivePool].[TotalOutcomes] * [NegativePool].[TotalOutcomes]) - [SuccessOutcomes]) As [FailureOutcomes]
	  ,(CAST((([PositivePool].[TotalOutcomes] * [NegativePool].[TotalOutcomes]) - [SuccessOutcomes]) as decimal(36,2)) / ([PositivePool].[TotalOutcomes] * [NegativePool].[TotalOutcomes])) AS [Failure Rate]

      ,[AdvantageOutcomes]
	  ,(CAST([AdvantageOutcomes] as decimal(36,2)) / ([PositivePool].[TotalOutcomes] * [NegativePool].[TotalOutcomes])) AS [Advantage Rate]
      ,[ThreatOutcomes]
	  ,(CAST([ThreatOutcomes] as decimal(36,2)) / ([PositivePool].[TotalOutcomes] * [NegativePool].[TotalOutcomes])) AS [Threat Rate]

      ,[DespairOutcomes]
	  ,(CAST([DespairOutcomes] as decimal(36,2)) / ([PositivePool].[TotalOutcomes] * [NegativePool].[TotalOutcomes])) AS [Despair Rate]
      ,[TriumphOutcomes]
	  ,(CAST([TriumphOutcomes] as decimal(36,2)) / ([PositivePool].[TotalOutcomes] * [NegativePool].[TotalOutcomes])) AS [Triumph Rate]

  FROM [DataGenerator].[dbo].[PoolCombination]
	LEFT JOIN [DataGenerator].[dbo].[Pool] AS [PositivePool] ON ( [PoolCombination].[PositivePoolId] = [PositivePool].[PoolId])
	LEFT JOIN [DataGenerator].[dbo].[Pool] AS [NegativePool] ON ( [PoolCombination].[NegativePoolId] = [NegativePool].[PoolId])

ORDER BY [Combination Name]