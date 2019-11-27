# Welcome to Sistema de votaos de aulas

![Version](https://img.shields.io/badge/version-alpha-blue.svg?cacheSeconds=2592000)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](#)

> Trabalho realizado pelos alunos do curso de Sistemas de informação, na disciplina Programação para Web 2

## Install

- Realizar o clone do repositório
- Entrar na pasta principal e executar o comando:

```sh
 docker build --pull -t votoifs .
 ```

## Para testar/Executar

- Com a imagem compilada, execute:

```sh
docker run --rm -it -p 8000:80 -v <pasta na maquina>:/app/Database
```


- Substituir:
  - `<pasta na maquina>` por um caminho da sua máquina
- Usuários:
  - Administrador
    - Login: administrador@localhost.com
	- Senha: 123qwe

  - Professor 
    - Login: professor@localhost.com
	- Senha: 123qwe

## Autores

👤 **Turma da disciplina de Programação Web 2 do IFS - Ano 2019.2**


## Contribua com o projeto

- Que tal resolver uma das nossas pendências ou então testar o sistema e reportar melhorias? Acesse [aqui](https://github.com/CBSIIFSLagarto/VotoIFS/issues) para saber mais.

***
_This README was generated with ❤️ by [readme-md-generator](https://github.com/kefranabg/readme-md-generator)_