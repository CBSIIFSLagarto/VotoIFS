# Welcome to Sistema de votos de aulas

![Version](https://img.shields.io/badge/version-alpha-blue.svg?cacheSeconds=2592000)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](#)

> Trabalho realizado pelos alunos do curso de Sistemas de informação, na disciplina Programação para Web 2

## Compilação/instalação

- Realizar o clone do repositório
- Entrar na pasta principal e executar o comando:

```sh
 docker build --pull -t votoifs .
 ```

## Para Executar

- Com a imagem compilada, execute:

```sh
docker run --name votoifs \
    --rm -it \
    -p 8000:80 \
    --mount type=bind,source=<local_na_maquina_fisica>,target=/app/DataBase \
    votoifs
```
> Substituir:
>  - `<local_na_maquina_fisica>` por um caminho da sua máquina.

Exemplo:

```sh
docker run --name votoifs --rm -it \
      -p 8000:80 \
      --mount type=bind,source=/home/pi/Documents/DataBase,target=/app/DataBase \
      votoifs
```

# Para Testar

Abrir a URL `http://<ip>:8000` e fazer login com um dos perfis abaixo:

Perfil | Login | Senha 
-------|:-----:|:----:|
👮🏻 Administrador | administrador@localhost.com | 123qwe
👨‍ Professor | professor@localhost.com | 123qwe

## Autores

👤 **Turma da disciplina de Programação Web 2 do IFS - Ano 2019.2**


## Contribua com o projeto

- Que tal resolver uma das nossas pendências ou então testar o sistema e reportar melhorias? Acesse [aqui](https://github.com/CBSIIFSLagarto/VotoIFS/issues) para saber mais.

***
_This README was generated with ❤️ by [readme-md-generator](https://github.com/kefranabg/readme-md-generator)_
