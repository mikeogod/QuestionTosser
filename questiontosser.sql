-- phpMyAdmin SQL Dump
-- version 4.0.4.1
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Apr 17, 2015 at 04:47 AM
-- Server version: 5.5.32
-- PHP Version: 5.4.19

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `questiontosser`
--
CREATE DATABASE IF NOT EXISTS `questiontosser` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `questiontosser`;

-- --------------------------------------------------------

--
-- Table structure for table `class`
--

CREATE TABLE IF NOT EXISTS `class` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `prof_id` int(11) NOT NULL,
  `code` varchar(10) NOT NULL DEFAULT 'free',
  `name` varchar(50) NOT NULL DEFAULT 'Classroom',
  `prof_connection_id` varchar(200) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `class_prof_id` (`prof_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=8 ;

--
-- Dumping data for table `class`
--

INSERT INTO `class` (`id`, `prof_id`, `code`, `name`, `prof_connection_id`) VALUES
(5, 4, '123', 'Class', '123'),
(6, 4, '123', 'Class', '123'),
(7, 4, '123', 'Class', '123');

-- --------------------------------------------------------

--
-- Table structure for table `professor`
--

CREATE TABLE IF NOT EXISTS `professor` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(200) COLLATE utf8_unicode_ci NOT NULL DEFAULT 'Unknown',
  `username` varchar(200) COLLATE utf8_unicode_ci NOT NULL,
  `password` varchar(100) COLLATE utf8_unicode_ci NOT NULL,
  `salt` varchar(20) COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci AUTO_INCREMENT=6 ;

--
-- Dumping data for table `professor`
--

INSERT INTO `professor` (`id`, `name`, `username`, `password`, `salt`) VALUES
(4, 'Plotka', 'plot', '1z+5dgAHfqWTy2BeWGVFjymD74mP7ityvqGfMvEc/Qg=', 'XOIBXGbpkw=='),
(5, 'oops', 'oops', 'r8PeHNyDEvvz7kY/03hP7PoYSHofa0E1McycT26WuYs=', '8cMgilk=');

-- --------------------------------------------------------

--
-- Table structure for table `student`
--

CREATE TABLE IF NOT EXISTS `student` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(200) COLLATE utf8_unicode_ci NOT NULL,
  `password` varchar(200) COLLATE utf8_unicode_ci NOT NULL,
  `salt` varchar(20) COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci AUTO_INCREMENT=8 ;

--
-- Dumping data for table `student`
--

INSERT INTO `student` (`id`, `username`, `password`, `salt`) VALUES
(4, 'Kirsti', 'wiR5nWencoZ97aSIsSXgPLmRcVtV9vKsJhuTtuVHzJU=', 'mv9/953oSQ=='),
(5, 'Mike', 'fM3w/5ogqnmx4svrwNWQyrMxYIcAKAGDY5qKDkTtJaw=', 'TpcaqK21'),
(7, 'sosmart', 'fbtjzTwfB6QFMSbozwoHqFfKfWIwbZRTlpXrtFDsqqQ=', 'yBKhc08=');

--
-- Constraints for dumped tables
--

--
-- Constraints for table `class`
--
ALTER TABLE `class`
  ADD CONSTRAINT `class_prof_id` FOREIGN KEY (`prof_id`) REFERENCES `professor` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
