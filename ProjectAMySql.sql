-- MySQL dump 10.13  Distrib 8.0.33, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: projectadb26
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `advisor`
--

DROP TABLE IF EXISTS `advisor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `advisor` (
  `Id` int NOT NULL,
  `Designation` int NOT NULL,
  `Salary` decimal(18,0) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Advisor_Lookup` (`Designation`),
  CONSTRAINT `FK_Advisor_Lookup` FOREIGN KEY (`Designation`) REFERENCES `lookup` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `advisor`
--

LOCK TABLES `advisor` WRITE;
/*!40000 ALTER TABLE `advisor` DISABLE KEYS */;
/*!40000 ALTER TABLE `advisor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `evaluation`
--

DROP TABLE IF EXISTS `evaluation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `evaluation` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(200) NOT NULL,
  `TotalMarks` int NOT NULL,
  `TotalWeightage` int NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `evaluation`
--

LOCK TABLES `evaluation` WRITE;
/*!40000 ALTER TABLE `evaluation` DISABLE KEYS */;
/*!40000 ALTER TABLE `evaluation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `group`
--

DROP TABLE IF EXISTS `group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `group` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Created_On` date NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `group`
--

LOCK TABLES `group` WRITE;
/*!40000 ALTER TABLE `group` DISABLE KEYS */;
INSERT INTO `group` VALUES (21,'2026-02-21');
/*!40000 ALTER TABLE `group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `groupevaluation`
--

DROP TABLE IF EXISTS `groupevaluation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `groupevaluation` (
  `GroupId` int NOT NULL,
  `EvaluationId` int NOT NULL,
  `ObtainedMarks` int NOT NULL,
  `EvaluationDate` datetime(3) NOT NULL,
  PRIMARY KEY (`GroupId`,`EvaluationId`),
  KEY `FK_GroupEvaluation_Evaluation` (`EvaluationId`),
  CONSTRAINT `FK_GroupEvaluation_Evaluation` FOREIGN KEY (`EvaluationId`) REFERENCES `evaluation` (`Id`),
  CONSTRAINT `FK_GroupEvaluation_Group` FOREIGN KEY (`GroupId`) REFERENCES `group` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groupevaluation`
--

LOCK TABLES `groupevaluation` WRITE;
/*!40000 ALTER TABLE `groupevaluation` DISABLE KEYS */;
/*!40000 ALTER TABLE `groupevaluation` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `groupproject`
--

DROP TABLE IF EXISTS `groupproject`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `groupproject` (
  `ProjectId` int NOT NULL,
  `GroupId` int NOT NULL,
  `AssignmentDate` datetime(3) NOT NULL,
  PRIMARY KEY (`ProjectId`,`GroupId`),
  KEY `FK_GroupProject_Group` (`GroupId`),
  CONSTRAINT `FK_GroupProject_Group` FOREIGN KEY (`GroupId`) REFERENCES `group` (`Id`),
  CONSTRAINT `FK_GroupProject_Project` FOREIGN KEY (`ProjectId`) REFERENCES `project` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groupproject`
--

LOCK TABLES `groupproject` WRITE;
/*!40000 ALTER TABLE `groupproject` DISABLE KEYS */;
/*!40000 ALTER TABLE `groupproject` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `groupstudent`
--

DROP TABLE IF EXISTS `groupstudent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `groupstudent` (
  `GroupId` int NOT NULL,
  `StudentId` int NOT NULL,
  `Status` int NOT NULL,
  `AssignmentDate` datetime(3) NOT NULL,
  PRIMARY KEY (`GroupId`,`StudentId`),
  KEY `FK_GroupStudents_Lookup` (`Status`),
  KEY `FK_ProjectStudents_Student` (`StudentId`),
  CONSTRAINT `FK_GroupStudents_Lookup` FOREIGN KEY (`Status`) REFERENCES `lookup` (`Id`),
  CONSTRAINT `FK_ProjectStudents_Group` FOREIGN KEY (`GroupId`) REFERENCES `group` (`Id`),
  CONSTRAINT `FK_ProjectStudents_Student` FOREIGN KEY (`StudentId`) REFERENCES `student` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `groupstudent`
--

LOCK TABLES `groupstudent` WRITE;
/*!40000 ALTER TABLE `groupstudent` DISABLE KEYS */;
/*!40000 ALTER TABLE `groupstudent` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `lookup`
--

DROP TABLE IF EXISTS `lookup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `lookup` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Value` varchar(100) NOT NULL,
  `Category` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `lookup`
--

LOCK TABLES `lookup` WRITE;
/*!40000 ALTER TABLE `lookup` DISABLE KEYS */;
INSERT INTO `lookup` VALUES (1,'Male','GENDER'),(2,'Female','GENDER'),(3,'Active','STATUS'),(4,'InActive','STATUS'),(6,'Professor','DESIGNATION'),(7,'Associate Professor','DESIGNATION'),(8,'Assisstant Professor','DESIGNATION'),(9,'Lecturer','DESIGNATION'),(10,'Industry Professional','DESIGNATION'),(11,'Main Advisor','ADVISOR_ROLE'),(12,'Co-Advisror','ADVISOR_ROLE'),(14,'Industry Advisor','ADVISOR_ROLE');
/*!40000 ALTER TABLE `lookup` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `person`
--

DROP TABLE IF EXISTS `person`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `person` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) NOT NULL,
  `LastName` varchar(100) DEFAULT NULL,
  `Contact` varchar(20) DEFAULT NULL,
  `Email` varchar(30) NOT NULL,
  `DateOfBirth` datetime(3) DEFAULT NULL,
  `Gender` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Person_Lookup` (`Gender`),
  CONSTRAINT `FK_Person_Lookup` FOREIGN KEY (`Gender`) REFERENCES `lookup` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `person`
--

LOCK TABLES `person` WRITE;
/*!40000 ALTER TABLE `person` DISABLE KEYS */;
/*!40000 ALTER TABLE `person` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `project`
--

DROP TABLE IF EXISTS `project`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `project` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Description` longtext,
  `Title` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `project`
--

LOCK TABLES `project` WRITE;
/*!40000 ALTER TABLE `project` DISABLE KEYS */;
/*!40000 ALTER TABLE `project` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `projectadvisor`
--

DROP TABLE IF EXISTS `projectadvisor`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `projectadvisor` (
  `AdvisorId` int NOT NULL,
  `ProjectId` int NOT NULL,
  `AdvisorRole` int NOT NULL,
  `AssignmentDate` datetime(3) NOT NULL,
  PRIMARY KEY (`AdvisorId`,`ProjectId`),
  KEY `FK_ProjectAdvisor_Lookup` (`AdvisorRole`),
  KEY `FK_ProjectAdvisor_Project` (`ProjectId`),
  CONSTRAINT `FK_ProjectAdvisor_Lookup` FOREIGN KEY (`AdvisorRole`) REFERENCES `lookup` (`Id`),
  CONSTRAINT `FK_ProjectAdvisor_Project` FOREIGN KEY (`ProjectId`) REFERENCES `project` (`Id`),
  CONSTRAINT `FK_ProjectTeachers_Teacher` FOREIGN KEY (`AdvisorId`) REFERENCES `advisor` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `projectadvisor`
--

LOCK TABLES `projectadvisor` WRITE;
/*!40000 ALTER TABLE `projectadvisor` DISABLE KEYS */;
/*!40000 ALTER TABLE `projectadvisor` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `student`
--

DROP TABLE IF EXISTS `student`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `student` (
  `Id` int NOT NULL,
  `RegistrationNo` varchar(20) NOT NULL,
  PRIMARY KEY (`Id`),
  CONSTRAINT `FK_Student_Person` FOREIGN KEY (`Id`) REFERENCES `person` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `student`
--

LOCK TABLES `student` WRITE;
/*!40000 ALTER TABLE `student` DISABLE KEYS */;
/*!40000 ALTER TABLE `student` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-03-10  0:19:46
