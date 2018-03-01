Event.observe(document, "dom:loaded", function () {
    scAttachEvents();
    var sortList = $("sort-list");
    if (sortList) {
        Sortable.create("sort-list", {
            scroll: "MainContainer", elements: $$(".sort-item.editable"), format: /^([0-9a-zA-Z]{32})$/, onUpdate: function () {
                scPersistDelete();
            }
        });
    }
});

function scAttachEvents() {
    $$(".sort-item.editable").each(function (item) {
        item.on("click", onItemClick);
        if (scForm.browser.isIE) {
            item.on("mouseenter", function () { this.addClassName("hover"); });
            item.on("mouseleave", function () { this.removeClassName("hover"); });
        }
    });

    Event.observe(document.body, "click", function () {
        scClearSelectedItem();
        scUpdateMoveButtonsState();
    });
};

function onItemClick(e) {
    e.stop();
    scClearSelectedItem();
    this.addClassName("selected");

    scUpdateMoveButtonsState();
};

function scClearSelectedItem() {
    var selectedItem = scGetSelectedItem();
    if (selectedItem) {
        selectedItem.removeClassName("selected");
    };
};

function scGetSelectedItem() {
    return $$(".selected")[0];
};

function scUpdateMoveButtonsState() {
    var Delete = $("Delete");
    var selectedItem = scGetSelectedItem();
    if (!selectedItem) {
        Delete.disable();
        return;
    }
};

function scPersistDelete() {
    var deleteItem = $("deleteItem");
    if (!deleteItem) {
        deleteItem = new Element("input", { type: "hidden", id: "deleteItem" });
        var form = document.forms[0];
        if (!form) {
            return;
        }

        form.appendChild(deleteItem);
    }
    scGetSelectedItem().addClassName("deleted");
    var ids = $$(".deleted").map(function (item) { return item.id; }) || [];
    var serialized = ids.join("|") || "";
    deleteItem.value = serialized;
};

function scDelete() {
    scPersistDelete();
    return false;
}