-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Czas generowania: 18 Kwi 2023, 15:45
-- Wersja serwera: 10.4.27-MariaDB
-- Wersja PHP: 8.1.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Baza danych: `szl_odlot`
--

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `lotniska_odlotowe`
--

CREATE TABLE `lotniska_odlotowe` (
  `id` int(4) NOT NULL,
  `nazwa` varchar(40) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;

--
-- Zrzut danych tabeli `lotniska_odlotowe`
--

INSERT INTO `lotniska_odlotowe` (`id`, `nazwa`) VALUES
(1, 'Francja - paryz'),
(2, 'Polska - warszawa'),
(3, 'Niemcy - berlin'),
(4, 'Japonia - tokyo');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `lotniska_przylotowe`
--

CREATE TABLE `lotniska_przylotowe` (
  `id` int(4) NOT NULL,
  `nazwa` varchar(40) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;

--
-- Zrzut danych tabeli `lotniska_przylotowe`
--

INSERT INTO `lotniska_przylotowe` (`id`, `nazwa`) VALUES
(1, 'Francja - paryz'),
(2, 'Polska - warszawa'),
(3, 'Niemcy - berlin'),
(4, 'Japonia - tokyo');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `samolot`
--

CREATE TABLE `samolot` (
  `id` int(4) NOT NULL,
  `nazwa` varchar(20) NOT NULL,
  `model` varchar(20) NOT NULL,
  `ilosc_max_miejsc` int(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;

--
-- Zrzut danych tabeli `samolot`
--

INSERT INTO `samolot` (`id`, `nazwa`, `model`, `ilosc_max_miejsc`) VALUES
(1, 'FR892-M', 'Boeing 777', 312),
(2, 'KLO-0-BN', 'Boeing 747', 280);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `trasa`
--

CREATE TABLE `trasa` (
  `id` int(11) NOT NULL,
  `id_lotniska_odlot` int(4) NOT NULL,
  `id_lotniska_przylot` int(4) NOT NULL,
  `id_samolotu` int(4) NOT NULL,
  `cena` int(5) NOT NULL,
  `ilosc_miejsc` int(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;

--
-- Zrzut danych tabeli `trasa`
--

INSERT INTO `trasa` (`id`, `id_lotniska_odlot`, `id_lotniska_przylot`, `id_samolotu`, `cena`, `ilosc_miejsc`) VALUES
(1, 1, 2, 1, 2056, 44),
(2, 3, 4, 2, 4986, 275),
(3, 2, 4, 1, 2899, 94);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `user`
--

CREATE TABLE `user` (
  `id` int(11) NOT NULL,
  `login` varchar(20) NOT NULL,
  `password` varchar(20) NOT NULL,
  `level` int(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;

--
-- Zrzut danych tabeli `user`
--

INSERT INTO `user` (`id`, `login`, `password`, `level`) VALUES
(1, 'admin', 'admin', 1),
(2, 'miszelin', 'forewerjang', 1),
(3, '1', '1', 1),
(4, 'lipa', 'nolipa', 1),
(5, 'igor', 'ronaldo', 1);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `user_trasa`
--

CREATE TABLE `user_trasa` (
  `id` int(4) NOT NULL,
  `ID_trasa` int(4) NOT NULL,
  `ID_user` int(4) NOT NULL,
  `ilosc_biletow` int(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_polish_ci;

--
-- Zrzut danych tabeli `user_trasa`
--

INSERT INTO `user_trasa` (`id`, `ID_trasa`, `ID_user`, `ilosc_biletow`) VALUES
(1, 1, 5, 4),
(2, 2, 5, 1),
(3, 1, 3, 3),
(4, 3, 3, 1),
(5, 1, 3, 12),
(6, 3, 1, 4),
(7, 1, 3, 20),
(8, 1, 3, 54),
(9, 2, 1, 2);

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `lotniska_odlotowe`
--
ALTER TABLE `lotniska_odlotowe`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `lotniska_przylotowe`
--
ALTER TABLE `lotniska_przylotowe`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `samolot`
--
ALTER TABLE `samolot`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `trasa`
--
ALTER TABLE `trasa`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`);

--
-- Indeksy dla tabeli `user_trasa`
--
ALTER TABLE `user_trasa`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT dla zrzuconych tabel
--

--
-- AUTO_INCREMENT dla tabeli `lotniska_odlotowe`
--
ALTER TABLE `lotniska_odlotowe`
  MODIFY `id` int(4) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT dla tabeli `lotniska_przylotowe`
--
ALTER TABLE `lotniska_przylotowe`
  MODIFY `id` int(4) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT dla tabeli `samolot`
--
ALTER TABLE `samolot`
  MODIFY `id` int(4) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT dla tabeli `trasa`
--
ALTER TABLE `trasa`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT dla tabeli `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT dla tabeli `user_trasa`
--
ALTER TABLE `user_trasa`
  MODIFY `id` int(4) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
