using Godot;
using System.Collections.Generic;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.UI;

public partial class TypewriterLabel : RichTextLabel, IEventHandler<DialogBoxTextChangedEvent>, IEventHandler<DialogBoxClearEvent>
{
    [Export] private float characterDelay;

    private double elapsedTimeSinceLastCharacter;
    private bool typing = false;
    private int shownCharacters = 0;

    private static readonly Dictionary<char, float> characterDelayMultipliers = new Dictionary<char, float>()
    {
        { ' ', 1.25f },
        { '\n', 2.0f },
        { '.', 1.25f },
        { '!', 1.25f },
        { '?', 1.25f },
        { ',', 1.25f }
    };
    
    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler<DialogBoxTextChangedEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<DialogBoxClearEvent>(this);
    }

    public void Handle(DialogBoxTextChangedEvent evt)
    {
        VisibleCharacters = 0;
        Text = evt.Text;

        if (evt.Instant)
        {
            VisibleCharacters = -1;
            typing = false;
            OnFinished();
        }
        else
        {
            // Start typewriter effect
            elapsedTimeSinceLastCharacter = 0;
            shownCharacters = 0;
            typing = true;
        }
    }

    public override void _Process(double delta)
    {
        if (typing)
        {
            if (shownCharacters >= Text.Length)
            {
                typing = false;
                OnFinished();
                return;
            }
            
            elapsedTimeSinceLastCharacter += delta;

            char nextChar = Text[shownCharacters];
            float nextCharDelay = characterDelayMultipliers.GetValueOrDefault(nextChar, 1.0f) * characterDelay;

            if (elapsedTimeSinceLastCharacter >= nextCharDelay)
            {
                shownCharacters++;
                elapsedTimeSinceLastCharacter -= nextCharDelay;
            }

            VisibleCharacters = shownCharacters;
        }
    }

    private void OnFinished()
    {
        Game.INSTANCE.EventBus.Post(new DialogBoxTypingFinishedEvent());
    }

    public void Handle(DialogBoxClearEvent evt)
    {
        typing = false;
        Text = string.Empty;
    }
}