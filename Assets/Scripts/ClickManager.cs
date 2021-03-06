﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClickManager : MonoBehaviour {

    private Draggable clicked_gate = null;
    private Draggable hovered_gate = null;

    public GameObject gate_context_menu;
    public GameObject connection_context_menu;
    public GameObject spawn_context_menu;
    public GameObject background_scroller_object;

    private GateContextManager gate_context_manager;
    private ConnectionContextManager connection_context_manager;
    private SpawnContextManager spawn_context_manager;
    private BackgroundScroller background_scroller;

    private void Start() {
        gate_context_manager = gate_context_menu.GetComponent<GateContextManager>();
        connection_context_manager = connection_context_menu.GetComponent<ConnectionContextManager>();
        spawn_context_manager = spawn_context_menu.GetComponent<SpawnContextManager>();
        background_scroller = background_scroller_object.GetComponent<BackgroundScroller>();
    }

    private void Update() {
        // Left click
        if (Input.GetMouseButtonDown(0)) {
            mouse_one_down();
            gate_context_manager.left_click();
            connection_context_manager.left_click();
            spawn_context_manager.left_click();
        }
        if (Input.GetMouseButton(0)) {
            mouse_one_pressed();
        }
        if (Input.GetMouseButtonUp(0)) {
            mouse_one_up();
        }
        // Right click
        if (Input.GetMouseButtonDown(1)) {
            gate_context_manager.left_click();
            connection_context_manager.left_click();
            spawn_context_manager.left_click();
            mouse_two_down();
        }
        // Middle click
        if (Input.GetMouseButtonDown(2)) {
            background_scroller.start_scroll();
        }
        if (Input.GetMouseButton(2)) {
            background_scroller.scroll();
        }
    }

    private void mouse_one_down() {
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouse_pos, Vector2.zero);
        if (hit.collider != null) {
            if (hit.collider.gameObject.GetComponent<Gate>() != null) {
                clicked_gate = hit.transform.gameObject.GetComponent<Draggable>();
                clicked_gate.set_mouse_start_pos(hit.point);
                clicked_gate.select(true);
            }
        }
    }

    private void mouse_one_pressed() {
        if (clicked_gate != null) {
            Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clicked_gate.set_new_pos(mouse_pos);
            clicked_gate.gate.changed = true;
        }
    }

    private void mouse_one_up() {
        if (clicked_gate != null) {
            clicked_gate.select(false);
            clicked_gate = null;
        }
    }

    private void mouse_two_down() {
        bool is_hit = false;
        Vector3 mouse_world_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouse_world_pos, Vector2.zero);
        if (hit.collider != null) {
            Gate collided_gate = hit.collider.gameObject.GetComponent<Gate>();
            if (collided_gate != null) {
                gate_context_manager.open_menu(Input.mousePosition);
                gate_context_manager.set_selected(collided_gate);
                is_hit = true;
            }
        }
        else if (Physics.Raycast(mouse_world_pos, Vector3.forward, out RaycastHit hit_3d)) {
            if (hit_3d.collider != null) {
                Connection collided_connection = hit_3d.collider.gameObject.GetComponent<Connection>();
                if (collided_connection != null) {
                    if (!collided_connection.is_temporary) {
                        connection_context_manager.open_menu(Input.mousePosition);
                        connection_context_manager.set_selected(collided_connection);
                        is_hit = true;
                    }
                }
            }
        }
        if (!is_hit) {
            spawn_context_manager.open_menu(Input.mousePosition);
        }
    }
}
