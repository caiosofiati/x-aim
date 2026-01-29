# 🚀 Como Publicar uma Release no GitHub

Para que os usuários baixem apenas o executável (`X-Aim.exe`) sem precisar baixar o código fonte, você deve usar a funcionalidade **Releases** do GitHub.

## Passo a Passo

1.  **Gere a versão final:**
    Certifique-se de que já rodou o comando de publicação:
    ```powershell
    dotnet publish -c Release -o publish
    ```

2.  **Compacte o executável:**
    *   Vá até a pasta `publish`.
    *   Selecione todos os arquivos (o `X-Aim.exe` e as DLLs necessárias).
    *   Clique com o botão direito -> **Enviar para** -> **Pasta compactada (zip)**.
    *   Nomeie como `X-Aim_v1.0.zip` (ou a versão atual).

3.  **Crie a Release no GitHub:**
    *   Acesse seu repositório no GitHub.
    *   No menu lateral direito, clique em **Releases** (ou "Criar uma nova release").
    *   Clique em **Draft a new release**.
    *   **Choose a tag**: Crie uma tag como `v1.0.0`.
    *   **Release title**: Coloque um título, ex: `Lançamento Inicial - v1.0`.
    *   **Describe this release**: Descreva o que há de novo.

4.  **Anexar o Arquivo:**
    *   Na área **"Attach binaries by dropping them here..."**, arraste o seu arquivo `X-Aim_v1.0.zip` que você criou.

5.  **Publicar:**
    *   Clique em **Publish release**.

Agora, quem acessar seu repositório verá a seção **Releases** e poderá baixar o `.zip` contendo apenas o programa pronto para uso!
