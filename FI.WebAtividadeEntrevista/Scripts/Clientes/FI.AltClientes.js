﻿
$(document).ready(function () {

    if (obj) {
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

    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        $.ajax({
            url: '/Cliente/Alterar',
            method: "PUT",  
            data: {
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
            },
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
                }, 1000);
            }
        });
    });

    $('#openModalBtn').on('click', function () {
        var clienteId = $('#ClienteId').val();
        $('#ClienteIdModal').val(clienteId);
        $('#modalBeneficiario').modal('show');
        $('#NomeBeneficiario').val('');
        $('#CPFBeneficiario').val('');

        $('#incluirLink').off('click').on('click', function (event) {
            event.preventDefault();

            var clienteId = $('#ClienteIdModal').val();
            var nomeBeneficiario = $('#NomeBeneficiario').val();
            var cpfBeneficiario = $('#CPFBeneficiario').val();

            $.ajax({
                type: 'POST',
                url: '/Beneficiario/Incluir',
                data: {
                    ClienteId: clienteId,
                    Nome: nomeBeneficiario,
                    CPF: cpfBeneficiario
                },
                success: function (response) {
                    $('#modalBeneficiario').modal('hide');
                    ModalDialog("Beneficiário incluído com sucesso.", "SUCESSO!")

                    preencherTabela(response.beneficiarios);
                },
                error: function (xhr, status, error) {
                    alert('Ocorreu um erro ao incluir o beneficiário.');
                }
            });
        });


        $.ajax({
            url: '/Beneficiario/BeneficiarioList',
            type: 'POST',
            dataType: 'json',
            data: {
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

                    $('.alterar-btn').on('click', function () {
                        var clienteId = $(this).data('id');
                        window.location.href = '/Beneficiario/Alterar/' + clienteId;
                    });

                    $('.excluir-btn').on('click', function () {
                        var clienteId = $(this).data('id');
                        if (confirm("Tem certeza que deseja excluir este Beneficiãrio?")) {
                            $.ajax({
                                url: '/Beneficiario/Excluir/' + clienteId,
                                type: 'DELETE',
                                success: function (response) {
                                    if (response.success) {
                                        ModalDialog("Beneficiário excluído com sucesso.", "SUCESSO!")
                                        $('#openModalBtn').click();
                                    } else {
                                        ModalDialog("Ocorreu um erro", "ERRO!");
                                    }
                                },
                                error: function (error) {
                                    ModalDialog("Ocorreu um erro", error.responseText);
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


})

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}
