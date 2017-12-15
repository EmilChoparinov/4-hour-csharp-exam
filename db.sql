-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema DojoActivities
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema DojoActivities
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `DojoActivities` DEFAULT CHARACTER SET utf8 ;
USE `DojoActivities` ;

-- -----------------------------------------------------
-- Table `DojoActivities`.`Users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DojoActivities`.`Users` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `FirstName` VARCHAR(255) NULL,
  `LastName` VARCHAR(255) NULL,
  `Email` VARCHAR(255) NULL,
  `Password` VARCHAR(255) NULL,
  `CreatedAt` DATETIME NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `DojoActivities`.`Activities`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DojoActivities`.`Activities` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `ActivityName` VARCHAR(255) NULL,
  `Description` TEXT NULL,
  `ActivityDate` DATETIME NULL,
  `Duration` VARCHAR(255) NULL,
  `CoordinatorId` INT NOT NULL,
  PRIMARY KEY (`Id`, `CoordinatorId`),
  INDEX `fk_Activities_Users1_idx` (`CoordinatorId` ASC),
  CONSTRAINT `fk_Activities_Users1`
    FOREIGN KEY (`CoordinatorId`)
    REFERENCES `DojoActivities`.`Users` (`Id`)
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `DojoActivities`.`Participants`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `DojoActivities`.`Participants` (
  `ActivityId` INT NOT NULL,
  `UserId` INT NOT NULL,
  PRIMARY KEY (`ActivityId`, `UserId`),
  INDEX `fk_Activities_has_Users_Users1_idx` (`UserId` ASC),
  INDEX `fk_Activities_has_Users_Activities_idx` (`ActivityId` ASC),
  CONSTRAINT `fk_Activities_has_Users_Activities`
    FOREIGN KEY (`ActivityId`)
    REFERENCES `DojoActivities`.`Activities` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Activities_has_Users_Users1`
    FOREIGN KEY (`UserId`)
    REFERENCES `DojoActivities`.`Users` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
