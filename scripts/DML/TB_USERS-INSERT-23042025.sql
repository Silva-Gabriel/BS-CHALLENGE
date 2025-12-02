INSERT INTO TB_GENDER (ID, CODE, DESCRIPTION) VALUES
    (1,  'MALE',                  'Masculino'),
    (2,  'FEMALE',                'Feminino'),
    (3,  'NON_BINARY',            'Não binário'),
    (4,  'AGENDER',               'Sem gênero (Agênero)'),
    (5,  'BIGENDER',              'Bigênero'),
    (6,  'GENDERFLUID',           'Gênero fluido'),
    (7,  'GENDERQUEER',           'Gênero queer'),
    (8,  'TRANS_MALE',            'Homem trans'),
    (9,  'TRANS_FEMALE',          'Mulher trans'),
    (10, 'TRANS',                 'Pessoa trans'),
    (11, 'CIS_MALE',              'Homem cisgênero'),
    (12, 'CIS_FEMALE',            'Mulher cisgênero'),
    (13, 'ANDROGYNE',             'Andrógino'),
    (14, 'INTERSEX',              'Intersexo'),
    (15, 'DEMIMAN',               'Demihomem'),
    (16, 'DEMIWOMAN',             'Demimulher'),
    (17, 'TWO_SPIRIT',            'Two-Spirit'),
    (18, 'QUESTIONING',           'Questionando / em dúvida'),
    (19, 'OTHER',                 'Outro'),
    (20, 'NOT_DISCLOSED',         'Prefere não informar');

INSERT INTO TB_ACCESS_GROUP (ID, NAME)
VALUES 
    (1, 'ADMIN'),
    (2, 'USER'),
    (3, 'MANAGER'),
    (4, 'GUEST');

INSERT INTO TB_PERSON (FULL_NAME, DOCUMENT_NUMBER, BIRTH_DATE, GENDER_ID, ACTIVE)
VALUES
    ('Gabriel Santana', '12345678901', '2000-07-08', 1, 1),
        ('Maria Oliveira', '98765432100', '1995-08-20', 2, 1);

INSERT INTO TB_USERS (USERNAME, PASSWORD_HASH, ACCESS_GROUP, PERSON_ID)
VALUES
    ('gabriel.santana', '$2y$10$BTWilSQs2ytMoTORykDi9e2hQJ0wm.swJa8zls32KemvUuoKcG0FS', '1', 1), -- admin123
    ('maria.oliveira',  '$2y$10$RiSoDDjdVWcoy12kh0A08e9vAd7ULqgkgFITFStzyz7J0lNDKi/r6',  '1', 2);  -- teste123

-- Contatos do Gabriel (ID = 1)
INSERT INTO TB_PERSON_CONTACT (PERSON_ID, TYPE, VALUE, IS_PRIMARY)
VALUES
    (1, 'EMAIL',  'gabriel.santana@example.com', 1),
    (1, 'MOBILE', '(11) 99815-2812',             1),
    (1, 'PHONE',  '(11) 4002-8922',              0);

-- Contatos da Maria (ID = 2)
INSERT INTO TB_PERSON_CONTACT (PERSON_ID, TYPE, VALUE, IS_PRIMARY)
VALUES
    (2, 'EMAIL',  'maria.oliveira@example.com',  1),
    (2, 'MOBILE', '(11) 98888-0000',             1);

-- Endereço do Gabriel (ID = 1)
INSERT INTO TB_PERSON_ADDRESS (
    PERSON_ID, STREET, NUMBER, COMPLEMENT, NEIGHBORHOOD,
    CITY, STATE, COUNTRY, ZIP_CODE, IS_PRIMARY
)
VALUES
    (1, 'Rua das Flores', '123', 'Ap 45', 'Centro',
     'Guarulhos', 'SP', 'Brasil', '07000-000', 1);

-- Endereço da Maria (ID = 2)
INSERT INTO TB_PERSON_ADDRESS (
    PERSON_ID, STREET, NUMBER, COMPLEMENT, NEIGHBORHOOD,
    CITY, STATE, COUNTRY, ZIP_CODE, IS_PRIMARY
)
VALUES
    (2, 'Avenida Paulista', '1000', 'Conjunto 101', 'Bela Vista',
     'São Paulo', 'SP', 'Brasil', '01310-000', 1);