INSERT INTO [dbo].[AspNetRoles] VALUES (
'9a492d20-57b7-477b-9966-c4457a1f703a','Administrador','ADMINISTRADOR','3001ee7f-b748-4f45-bdb4-3200fc22b845');

INSERT INTO [dbo].[AspNetUsers] VALUES (
'de897bf8-283c-4819-9d06-990e307c1520','admin@ifs.edu.br','admin@ifs.edu.br','admin@ifs.edu.br','admin@ifs.edu.br',
0,'AQAAAAEAACcQAAAAEHWjbOIHqtSUeaxyCj/PR6rYwJ/xgW8jl8TYjeC4+iDj4EsSkQogl1TYPTRDpUA/5g==',	
'WCSIGCB6MEQY4LMWJ7VFWYC43BJ6GNBD',
'4f1ea59f-a987-45cf-abe6-d9e456b43fae',	NULL,	0,	0,	NULL,	1,	1);

INSERT INTO [dbo].[AspNetUserRoles] VALUES ('de897bf8-283c-4819-9d06-990e307c1520','9a492d20-57b7-477b-9966-c4457a1f703a');