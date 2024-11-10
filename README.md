# FI_WebAtividadeEntrevista

## Implementações feitas:

### ➢ Implementação do CPF do cliente
Na tela de cadastramento/alteração de clientes, foi incluído um novo campo denominado **CPF**, que permite o cadastramento do CPF do cliente. 

**Pontos relevantes:**
- O novo campo segue o padrão visual dos demais campos da tela.
- O cadastramento do CPF é obrigatório.
- A formatação padrão de CPF é aplicada (999.999.999-99).
- O sistema valida se o dado informado é um CPF válido (conforme o cálculo padrão de verificação do dígito verificador de CPF).
- Não permite o cadastramento de um CPF já existente no banco de dados, evitando duplicidade.

### ➢ Implementação do botão Beneficiários
Na tela de cadastramento/alteração de clientes, foi incluído um novo botão denominado **Beneficiários**, que permite o cadastramento de beneficiários do cliente. Ao clicar, um pop-up é aberto para a inclusão do **CPF** e **Nome do beneficiário**. Além disso, um grid exibe os beneficiários já cadastrados, onde é possível realizar manutenção dos mesmos, incluindo alteração e exclusão.

**Pontos relevantes:**
- O novo botão e os campos seguem o padrão visual dos demais botões e campos da tela.
- O campo CPF possui formatação padrão (999.999.999-99).
- O sistema valida se o CPF do beneficiário é válido (conforme o cálculo padrão de verificação do dígito verificador de CPF).
- Não permite o cadastramento de mais de um beneficiário com o mesmo CPF para o mesmo cliente.
- O beneficiário é gravado na base de dados ao acionar o botão **Salvar** na tela de **Cadastrar Cliente**.
