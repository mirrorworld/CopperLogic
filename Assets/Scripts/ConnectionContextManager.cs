﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionContextManager : MonoBehaviour {

    private Connection selected_connection = null;
    private GameObject context_menu;
    private Vector2 offset;

    private void Start() {
        context_menu = transform.GetChild(0).gameObject;
        RectTransform rect = context_menu.GetComponent<RectTransform>();
        offset = Vector2.Scale(rect.sizeDelta * 0.5f, new Vector2(1.0f, -1.0f)) + new Vector2(5.0f, -5.0f);
    }

    public void set_selected(Connection connection) {
        selected_connection = connection;
    }

    public void open_menu(Vector2 mouse_pos) {
        context_menu.SetActive(true);
        context_menu.transform.position = mouse_pos + offset;
    }

    public void close_menu() {
        selected_connection = null;
        context_menu.SetActive(false);
    }

    public void delete_connection() {
        selected_connection.delete_from_parent();
        Destroy(selected_connection.gameObject);
    }
}
