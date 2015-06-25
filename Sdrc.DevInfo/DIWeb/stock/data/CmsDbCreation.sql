USE [master]
GO

IF NOT EXISTS(select * from sys.databases where NAME= 'DBName')
BEGIN
CREATE DATABASE [DBName] ON  PRIMARY 
( NAME = N'DBName', FILENAME = N'CmsDataBasePath\DBName.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DBName_log', FILENAME = N'CmsDataBasePath\DBName_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
END