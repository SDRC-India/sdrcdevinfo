IF NOT EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Data' AND IND.name = 'Index_UT_Data_ON_Indicator_NId' 
)
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Data_ON_Indicator_NId ON UT_Data
	(
		Indicator_NId ASC
	)
	INCLUDE(Data_NId, Data_Value, Unit_NId, Subgroup_Val_NId, Area_NId, TimePeriod_NId, Source_NId, isMRD)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Data_ON_Indicator_NId ON UT_Data
	(
		Indicator_NId ASC
	)
	INCLUDE(Data_NId, Data_Value, Unit_NId, Subgroup_Val_NId, Area_NId, TimePeriod_NId, Source_NId, isMRD)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = ON, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
GO

--SPSEPARATOR--

IF NOT EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Data' AND IND.name = 'Index_UT_Data_ON_Unit_NId' 
)
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Data_ON_Unit_NId ON UT_Data
	(
		Unit_NId ASC
	)
	INCLUDE(Data_NId, Data_Value, Indicator_NId, Subgroup_Val_NId, Area_NId, TimePeriod_NId, Source_NId, isMRD)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Data_ON_Unit_NId ON UT_Data
	(
		Unit_NId ASC
	)
	INCLUDE(Data_NId, Data_Value, Indicator_NId, Subgroup_Val_NId, Area_NId, TimePeriod_NId, Source_NId, isMRD)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = ON, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
GO

--SPSEPARATOR--

IF NOT EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Data' AND IND.name = 'Index_UT_Data_ON_Subgroup_Val_NId' 
)
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Data_ON_Subgroup_Val_NId ON UT_Data
	(
		Subgroup_Val_NId ASC
	)
	INCLUDE(Data_NId, Data_Value, Indicator_NId, Unit_NId, Area_NId, TimePeriod_NId, Source_NId, isMRD)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Data_ON_Subgroup_Val_NId ON UT_Data
	(
		Subgroup_Val_NId ASC
	)
	INCLUDE(Data_NId, Data_Value, Indicator_NId, Unit_NId, Area_NId, TimePeriod_NId, Source_NId, isMRD)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = ON, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
GO

--SPSEPARATOR--

IF NOT EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Data' AND IND.name = 'Index_UT_Data_ON_Area_NId' 
)
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Data_ON_Area_NId ON UT_Data
	(
		Area_NId ASC
	)
	INCLUDE(Data_NId, Data_Value, Indicator_NId, Unit_NId, Subgroup_Val_NId, TimePeriod_NId, Source_NId, isMRD)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Data_ON_Area_NId ON UT_Data
	(
		Area_NId ASC
	)
	INCLUDE(Data_NId, Data_Value, Indicator_NId, Unit_NId, Subgroup_Val_NId, TimePeriod_NId, Source_NId, isMRD)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = ON, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
GO

--SPSEPARATOR--

IF NOT EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Data' AND IND.name = 'Index_UT_Data_ON_TimePeriod_NId' 
)
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Data_ON_TimePeriod_NId ON UT_Data
	(
		TimePeriod_NId ASC
	)
	INCLUDE(Data_NId, Data_Value, Indicator_NId, Unit_NId, Subgroup_Val_NId, Area_NId, Source_NId, isMRD)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Data_ON_TimePeriod_NId ON UT_Data
	(
		TimePeriod_NId ASC
	)
	INCLUDE(Data_NId, Data_Value, Indicator_NId, Unit_NId, Subgroup_Val_NId, Area_NId, Source_NId, isMRD)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = ON, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
GO

--SPSEPARATOR--

IF NOT EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Indicator_Unit_Subgroup' AND IND.name = 'Index_UT_Indicator_Unit_Subgroup_ON_Indicator_NId' 
)
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Indicator_Unit_Subgroup_ON_Indicator_NId ON UT_Indicator_Unit_Subgroup
	(
		Indicator_NId ASC
	)
	INCLUDE(Unit_NId, Subgroup_Val_NId)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Indicator_Unit_Subgroup_ON_Indicator_NId ON UT_Indicator_Unit_Subgroup
	(
		Indicator_NId ASC
	)
	INCLUDE(Unit_NId, Subgroup_Val_NId)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = ON, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
GO

--SPSEPARATOR--

IF NOT EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Indicator_Unit_Subgroup' AND IND.name = 'Index_UT_Indicator_Unit_Subgroup_ON_Unit_NId' 
)
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Indicator_Unit_Subgroup_ON_Unit_NId ON UT_Indicator_Unit_Subgroup
	(
		Unit_NId ASC
	)
	INCLUDE(Indicator_NId, Subgroup_Val_NId)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Indicator_Unit_Subgroup_ON_Unit_NId ON UT_Indicator_Unit_Subgroup
	(
		Unit_NId ASC
	)
	INCLUDE(Indicator_NId, Subgroup_Val_NId)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = ON, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
GO

--SPSEPARATOR--

IF NOT EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_Indicator_Unit_Subgroup' AND IND.name = 'Index_UT_Indicator_Unit_Subgroup_ON_Subgroup_Val_NId' 
)
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Indicator_Unit_Subgroup_ON_Subgroup_Val_NId ON UT_Indicator_Unit_Subgroup
	(
		Subgroup_Val_NId ASC
	)
	INCLUDE(Indicator_NId, Unit_NId)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Indicator_Unit_Subgroup_ON_Subgroup_Val_NId ON UT_Indicator_Unit_Subgroup
	(
		Subgroup_Val_NId ASC
	)
	INCLUDE(Indicator_NId, Unit_NId)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = ON, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
GO

--SPSEPARATOR--

IF NOT EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_ic_IUS' AND IND.name = 'Index_UT_Indicator_Classifications_IUS_ON_IC_NId' 
)
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Indicator_Classifications_IUS_ON_IC_NId ON UT_ic_IUS
	(
		IC_NId ASC
	)
	INCLUDE(IUSNId)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Indicator_Classifications_IUS_ON_IC_NId ON UT_ic_IUS
	(
		IC_NId ASC
	)
	INCLUDE(IUSNId)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = ON, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
GO

--SPSEPARATOR--

IF NOT EXISTS 
(
	SELECT 1 FROM sys.Indexes IND INNER JOIN sys.tables TBL 
	ON IND.object_id = TBL.object_id
	WHERE TBL.name = 'UT_ic_IUS' AND IND.name = 'Index_UT_Indicator_Classifications_IUS_ON_IUSNId' 
)
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Indicator_Classifications_IUS_ON_IUSNId ON UT_ic_IUS
	(
		IUSNId ASC
	)
	INCLUDE(IC_NId)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
ELSE
BEGIN
	CREATE NONCLUSTERED INDEX Index_UT_Indicator_Classifications_IUS_ON_IUSNId ON UT_ic_IUS
	(
		IUSNId ASC
	)
	INCLUDE(IC_NId)
	WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = ON, IGNORE_DUP_KEY = OFF, ONLINE = OFF) 
END
