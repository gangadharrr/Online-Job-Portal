USE [Online Job Portal]

DROP TABLE JOBS;
DROP TABLE COMPANIES;
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='COMPANIES' and xtype='U')
CREATE TABLE COMPANIES
(
	COMPANY_ID INT IDENTITY(1,1) PRIMARY KEY ,
	COMPANY_NAME VARCHAR(30),
	COMPANY_DESCRIPTION VARCHAR(MAX),
)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='JOBS' and xtype='U')
CREATE TABLE JOBS
(
	JOB_ID INT IDENTITY(1,1) PRIMARY KEY ,
	JOB_NAME VARCHAR(30),
	JOB_DESCRIPTION VARCHAR(MAX),
	JOB_SALARY FLOAT,
	COMPANY_ID INT,
	FOREIGN KEY(COMPANY_ID) REFERENCES COMPANIES(COMPANY_ID) ON DELETE CASCADE
)
INSERT INTO COMPANIES VALUES
('Samsung','Manufactures Electronics'),
('Hewlet Packett','Digital Solutions');
INSERT INTO JOBS VALUES
('SDE-1','Developing Software products at beginner level',7.5,1),
('Software Tester-1','Testing and Debugging Software Products',3.5,1),
('SDE-1','Developing Software products at beginner level',8.5,2),
('Software Tester-1','Testing and Debugging Software Products',5.5,2);

select * from jobs;
select * from COMPANIES;

