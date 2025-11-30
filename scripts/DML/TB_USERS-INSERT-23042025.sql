-- Inserção de usuários com hashes novos (BCrypt, custo 12)
INSERT INTO TB_USERS (USERNAME, PASSWORD_HASH, ACTIVE, ACCESS_GROUP) VALUES 
('admin', '$2y$10$y3pjM8kD.voTFMS0qatp7OmUTrDjlA5uiT3sPeo6uClgmIrDF/Cba', '1', 1), -- senha123
('user', '$2y$10$RiSoDDjdVWcoy12kh0A08e9vAd7ULqgkgFITFStzyz7J0lNDKi/r6', '1', 2), -- teste123
('guest', '$2y$10$BTWilSQs2ytMoTORykDi9e2hQJ0wm.swJa8zls32KemvUuoKcG0FS', '1', 2), -- admin123

INSERT INTO TB_ACCESS_GROUP (ID, NAME) VALUES
(1, 'ADMIN'),
(2, 'USER'),
(3, 'GUEST');