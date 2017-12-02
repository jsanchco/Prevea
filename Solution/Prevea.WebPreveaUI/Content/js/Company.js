var CompanyViewModel = kendo.observable({
    container: null,

    tabGeneral_tbx_NamelId: "tabGeneral_tbx_Name",
    tabGeneral_tbx_AddresslId: "tabGeneral_tbx_Address",
    tabContactPerson_tbx_namelId: "tabContactPerson_tbx_name",
    tabContactPerson_tbx_lastNamelId: "tabContactPerson_tbx_lastName",
    tabContactPerson_tbx_phoneNumberlId: "tabContactPerson_tbx_phoneNumber",
    tabContactPerson_tbx_workStationlId: "tabContactPerson_tbx_workStation",
    tabContactPerson_tbx_emaillId: "tabContactPerson_tbx_email",

    name: null,
    address: null,

    contactPerson_name: null,
    contactPerson_lastName: null,
    contactPerson_phoneNumber: null,
    contactPerson_workStation: null,
    contactPerson_email: null,    
    
    contactPersons: new kendo.data.DataSource(),
    employees: [],    

    init: function (container) {
        this.container = container;

        kendo.bind(document.body.children, this);
        
        this.createGridContactPersons();
        //this.createTextBox();
        this.bindKendoUIWidgets();
    },

    bindKendoUIWidgets: function () {
        //kendo.bind($("#grid_contactPerson"), this.contactPersons);
        //kendo.bind($("#" + this.tabContactPerson_tbx_namelId), this.contactPerson_name);
    },

    createGridContactPersons: function () {
        $("#grid_contactPerson").kendoGrid({
            datasource: {
                //data: new kendo.data.DataSource({ data: this.contactPersons }),
                schema: {
                    model: {
                        fields: {
                            name: { type: "string" },
                            lastName: { type: "string" },
                            phoneNumber: { type: "string" },
                            workStation: { type: "string" },
                            email: { type: "string" }
                        }
                    }
                }
            },
            autobind: true,
            columns: [{
                field: "name",
                title: "Nombre",
                width: 150
            }, {
                field: "lastName",
                title: "Apellidos",
                width: 270
            }, {
                field: "phoneNumber",
                title: "Teléfono",
                width: 160
            }, {
                field: "workStation",
                title: "Puesto de Trabajo",
                width: 270
            }, {
                field: "email",
                title: "Email"
            }]
        });
    },

    createTextBox: function() {
        $("#" + this.tabContactPerson_tbx_namelId).kendoText();
    },

    click_btnAddPersonContact: function (e) {
        this.get("contactPersons").add({
            name: this.get("contactPerson_name"),
            lastName: this.get("contactPerson_lastName"),
            phoneNumber: this.get("contactPerson_phoneNumber"),
            workStation: this.get("contactPerson_workStation"),
            email: this.get("contactPerson_email")
        });

        //this.get("contactPersons").push({
        //    name: this.get("contactPerson_name"),
        //    lastName: this.get("contactPerson_lastName"),
        //    phoneNumber: this.get("contactPerson_phoneNumber"),
        //    workStation: this.get("contactPerson_workStation"),
        //    email: this.get("contactPerson_email")
        //});

       

        //var grid = $("#grid_contactPerson").data("kendoGrid");
        //grid.dataSource.data(this.contactPersons);
        //grid.dataSource.fetch();
    }
})