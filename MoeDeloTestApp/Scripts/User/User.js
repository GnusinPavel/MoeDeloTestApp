/**
 * Объект для работы с пользователями
 */
function User() {
    this.$form = $('#user-edit-form');
    this.$table = $('#user-table');
    this.$editDialog = $('#user-edit-dialog');
};

/**
 * Инициализация кнопки добавления
 */
User.prototype.initAddBtn = function () {
    var me = this;
    $('#add-user-btn').click(function () {
        $('#user-title-dialog').val('Добавить новового сотрудника');
        me.$form[0].reset();
        me.$editDialog.modal('show');
    });
}

/**
 * Инициализация таблицы с данными
 */
User.prototype.initTable = function () {
    this.$table.bootgrid({
        ajax: true,
        url: '/User/Filter',
        selection: true,
        sorting: true,
        rowSelect: true,
        formatters: {
            post: function(column, row) {
                return row['Post']['Name'];
            },
            commands: function(column, row) {
                var tpl = "<button type='button' class='btn btn-xs btn-default command-edit' data-row-id='" + row['Id'] + "\'>" +
                    "<span class='glyphicon glyphicon-pencil'></span>" +
                    "</button> " +
                    "<button type='button' class='btn btn-xs btn-default command-delete' data-row-id='" + row['Id'] + "'>" +
                    "<span class='glyphicon glyphicon-trash'></span>" +
                    "</button>";
                return tpl;
            }
        },
        labels: {
            all: 'Все',
            infos: 'с {{ctx.start}} по {{ctx.end}} из {{ctx.total}} строк',
            loading: 'Загрузка ...',
            noResults: 'Ничего не найдено',
            refresh: 'Обновить',
            search: 'Поиск'
        }
    });

    var me = this;
    this.$table.on('loaded.rs.jquery.bootgrid', function() {
        me.$table.find('.command-edit').click(function() {
            // редактирование сотрудника
            var row = me.getRow($(this).data('row-id'));
            me.fillEditForm(row);
            me.$editDialog.modal('show');
        });
        me.$table.find('.command-delete').click(function () {
            // удаление сотрудника
            var id = $(this).data('row-id');
            $('#user-remove-form').find('input[name=id]').val(id);
            $('#user-delete-dialog').modal('show');
        });
    });
}

/**
 * Получение строки с данными из таблицы
 */
User.prototype.getRow = function(id) {
    var rows = this.$table.data('.rs.jquery.bootgrid').currentRows;
    for (var i = 0, k = rows.length; i < k; i++) {
        var row = rows[i];
        if (row['Id'] === id) {
            return row;
        }
    }

    return null;
}

/**
 * Заполнить форму данными для редактирования
 */
User.prototype.fillEditForm = function (row) {
    if (row) {
        this.$form[0].reset();
        this.$form.find('#Id').val(row['Id']);
        this.$form.find('#FirstName').val(row['FirstName']);
        this.$form.find('#LastName').val(row['LastName']);
        this.$form.find('#Post_Id').val(row['Post']['Id']);
    }
}

/**
 * Инициализация кнопки сохрпнения
 */
User.prototype.initSaveBtn = function () {
    var me = this;
    $('#save-user-btn').click(function() {
        me.$form.submit();
    });
}

/**
 * Инициализация модуля
  */
User.prototype.init = function () {
    this.initTable();
    this.initAddBtn();
    this.initSaveBtn();
}
var user = new User();
user.init();