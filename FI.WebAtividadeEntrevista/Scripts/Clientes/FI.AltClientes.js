$(document).ready(function () {

    if (obj) {
        $('#formCadastro #IdCliente').val(obj.Id);
        $('#formCadastro #Nome').val(obj.Nome);
        $('#formCadastro #CEP').val(obj.CEP);
        $('#formCadastro #CPF').val(obj.CPF);
        $('#formCadastro #Email').val(obj.Email);
        $('#formCadastro #Sobrenome').val(obj.Sobrenome);
        $('#formCadastro #Nacionalidade').val(obj.Nacionalidade);
        $('#formCadastro #Estado').val(obj.Estado);
        $('#formCadastro #Cidade').val(obj.Cidade);
        $('#formCadastro #Logradouro').val(obj.Logradouro);
        $('#formCadastro #Telefone').val(obj.Telefone);
    }

    // Submit form for Cliente Alterar
    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        $.ajax({
            url: '/Cliente/Alterar',
            method: "PUT",
            contentType: "application/json",
            data: JSON.stringify({
                "Id": $(this).find("#IdCliente").val(),
                "Nome": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "CPF": $(this).find("#CPF").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val()
            }),
            error: function (r) {
                if (r.status === 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status === 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
            success: function (r) {
                ModalDialog("SUCESSO!", "Cliente foi atualizado!");
                setTimeout(function () {
                    window.location.href = '/Cliente';
                }, 5000);
            }
        });
    });

    // Open modal and include Beneficiario
    $('#openModalBtn').on('click', function () {
        var clienteId = obj.Id; // Certifique-se que obj.Id existe e tem o valor correto.
        $('#ClienteIdModal').val(clienteId);
        $('#modalBeneficiario').modal('show');
        $('#NomeBeneficiario').val('');
        $('#CPFBeneficiario').val('');

        $('#incluirLink').off('click').on('click', function (event) {
            event.preventDefault();

            var clienteId = $('#ClienteIdModal').val();  // Garantir que clienteId está correto
            var nomeBeneficiario = $('#NomeBeneficiario').val();
            var cpfBeneficiario = $('#CPFBeneficiario').val();

            if (!clienteId || !nomeBeneficiario || !cpfBeneficiario) {
                alert("Por favor, preencha todos os campos.");
                return;
            }

            $.ajax({
                type: 'POST',
                url: '/Beneficiario/Incluir',
                contentType: 'application/json',
                data: JSON.stringify({
                    ClienteId: clienteId,
                    Nome: nomeBeneficiario,
                    CPF: cpfBeneficiario
                }),
                success: function (response) {
                    if (response.success) {
                        $('#modalBeneficiario').modal('hide');
                    
                        
                        if (response.beneficiarios) {
                            preencherTabela(response.beneficiarios);
                        }
                    } else {
                        alert(response.message);
                    }
                },
                error: function (xhr, status, error) {
                    // Trata os erros do servidor com base no código de status
                    if (xhr.status === 400) {
                        alert("Erro: " + xhr.responseText);
                    } else if (xhr.status === 500) {
                        alert("Erro interno: " + xhr.responseText);
                    } else {
                        alert("Erro desconhecido: " + status);
                    }
                }
            });
        });

        // Carregar lista de Beneficiarios
        $.ajax({
            url: '/Beneficiario/BeneficiarioList',
            type: 'POST',
            dataType: 'json',
            data: {
                idCliente: clienteId,
                jtStartIndex: 0,
                jtPageSize: 10,
                jtSorting: 'Nome ASC'
            },
            success: function (response) {
                if (response.Result === "OK") {
                    $('#tabelaCorpo').empty();
                    response.Records.forEach(function (record) {
                        $('#tabelaCorpo').append(`
                            <tr>
                                <td>${record.CPF}</td>
                                <td>${record.Nome}</td>
                                <td>
                                    <button class="btn btn-primary btn-sm alterar-btn" style="margin-right: 5px;" data-id="${record.Id}">Alterar</button>
                                    <button class="btn btn-primary btn-sm excluir-btn" data-id="${record.Id}">Excluir</button>
                                </td>
                            </tr>
                        `);
                    });

                    // Event handlers for Alterar and Excluir
                    $('.alterar-btn').on('click', function () {
                        var clienteId = $(this).data('id');
                        window.location.href = '/Beneficiario/Alterar/' + clienteId;
                    });

                    $('.excluir-btn').on('click', function () {
                        var clienteId = $(this).data('id');
                        if (confirm("Tem certeza que deseja excluir este Beneficiário?")) {
                            $.ajax({
                                url: '/Beneficiario/Excluir/' + clienteId,
                                type: 'DELETE',
                                success: function (response) {
                                    if (response.success) {
                                        location.reload(); // Atualiza a página após a exclusão
                                    }
                                },
                                error: function () {
                                    location.reload(); // Atualiza a página em caso de erro
                                }
                            });
                        }
                    });
                } else {
                    console.error("Erro: " + response.Message);
                }
            },
            error: function (error) {
                console.error("Erro na requisição AJAX:", error);
            }
        });

        function preencherTabela(beneficiarios) {
            $('#tabelaCorpo').empty();

            $.each(beneficiarios, function (index, beneficiario) {
                var row = '<tr>' +
                    '<td>' + beneficiario.Nome + '</td>' +
                    '<td>' + beneficiario.CPF + '</td>' +
                    '<td>' +
                    '<button type="button" class="btn btn-primary btn-alterar" data-id="' + beneficiario.Id + '">Alterar</button>' +
                    '<button type="button" class="btn btn-primary btn-excluir">Excluir</button>' +
                    '</td>' +
                    '</tr>';
                $('#tabelaCorpo').append(row);
            });
        }

        $('#modalBeneficiario').on('show.bs.modal', function (event) {
            var clienteId = $('#ClienteId').val();
            $('#ClienteIdModal').val(clienteId);
        });
    });
});
