using System;
using DarkDefenders.ConsoleClient.Presenters;
using DarkDefenders.Domain.Interfaces;
using Infrastructure.DDDES.Implementations.Domain;
using Infrastructure.Runtime;
using Infrastructure.Util;
using Button = Infrastructure.Runtime.Button;

namespace DarkDefenders.ConsoleClient
{
    static class Program
    {
        private static readonly TimeSpan _heroRenderingPeriod = TimeSpan.FromSeconds(1.0 / 100);
        private static readonly TimeSpan _statsRenderingPeriod = TimeSpan.FromSeconds(1.0 / 100);
        private static readonly TimeSpan _keyboardUpdatePeriod = TimeSpan.FromSeconds(1.0 / 100);
        private static readonly TimeSpan _testHeroSpawnPeriod = TimeSpan.FromSeconds(1.0 / 500);

        private static readonly OnOffSwitch _limitFpsSwitch = new OnOffSwitch(true);

        private static readonly PeriodicExecutor _keyBoardExecutor = new PeriodicExecutor(_keyboardUpdatePeriod);
        private static readonly PeriodicExecutor _statsRenderingExecutor = new PeriodicExecutor(_statsRenderingPeriod);
        private static readonly SwitchablePeriodicExecutor _heroRenderingExecutor = new SwitchablePeriodicExecutor(_heroRenderingPeriod, true);
        private static readonly SwitchablePeriodicExecutor _testHeroSpawnExecutor = new SwitchablePeriodicExecutor(_testHeroSpawnPeriod, false);

        private static readonly PerformanceCounter _fpsCounter = new PerformanceCounter();
        private static readonly PerformanceCounter _eventsCounter = new PerformanceCounter();

        private static readonly OnOffSwitch _spawnHeroOnOffSwitch = new OnOffSwitch(true);
        private static readonly Button _killHeroesButton = new Button();
        private static readonly TimeSpan _elapsedLimit = TimeSpan.FromSeconds(1);

        static void Main()
        {
            var renderer = new GamePresenter();

            var rendererListener = DelegatingEventsListener.Create(renderer);

            var counter = new CountingEventsListener<IEventsReciever>();

            var composite = CompositeEventsListener.Create(rendererListener, counter);

            var eventDataListener = new EventDataListener(composite);

            var stopwatch = AutoResetStopwatch.StartNew();

            eventDataListener.ListenAsync();

            var loopRunner = new LoopRunner(100);

            loopRunner.Run(() => Frame(stopwatch, renderer, counter, eventDataListener));
        }

        private static void Frame(AutoResetStopwatch stopwatch, GamePresenter renderer, CountingEventsListener<IEventsReciever> counter, EventDataListener eventDataListener)
        {
            var elapsed = stopwatch.ElapsedSinceLastCall.LimitTo(_elapsedLimit);

            //_keyBoardExecutor.Tick(elapsed, () => ProcessKeyboard(game, player, world));
            //_testHeroSpawnExecutor.Tick(elapsed, world.SpawnHero);

            _heroRenderingExecutor.Tick(elapsed, renderer.RenderCreatures);
            _statsRenderingExecutor.Tick(elapsed, renderer.RenderCreatureState);
            _fpsCounter.Tick(elapsed, renderer.RenderFps);
            _eventsCounter.Tick(counter.EventsSinceLastCall, elapsed, renderer.RenderAverageEventsCount);

            eventDataListener.ProcessEvents();
        }

//        private static void ProcessKeyboard(IGame game, IPlayer player, IWorld world)
//        {
//            var leftIsPressed = NativeKeyboard.IsKeyDown(Keys.Left);
//            var rightIsPressed = NativeKeyboard.IsKeyDown(Keys.Right);
//            if (leftIsPressed && !rightIsPressed)
//            {
//                player.ChangeMovement(Movement.Left);
//            }
//            else if (rightIsPressed && !leftIsPressed)
//            {
//                player.ChangeMovement(Movement.Right);
//            }
//            else
//            {
//                player.ChangeMovement(Movement.Stop);
//            }
//
//            if (NativeKeyboard.IsKeyDown(Keys.Up))
//            {
//                player.Jump();
//            }
//
//            if (NativeKeyboard.IsKeyDown(Keys.LControlKey) || NativeKeyboard.IsKeyDown(Keys.RControlKey))
//            {
//                player.Fire();
//            }
//
//            _spawnHeroOnOffSwitch.State(NativeKeyboard.IsKeyDown(Keys.S), world.ChangeSpawnHeroes);
//            _killHeroesButton.State(NativeKeyboard.IsKeyDown(Keys.K), game.KillAllHeroes);
//
//            _limitFpsSwitch.State(NativeKeyboard.IsKeyDown(Keys.L));
//            _heroRenderingExecutor.State(NativeKeyboard.IsKeyDown(Keys.R));
//            _testHeroSpawnExecutor.State(NativeKeyboard.IsKeyDown(Keys.T));
//
//            if (NativeKeyboard.IsKeyDown(Keys.Escape))
//            {
//                _escape = true;
//            }
//        }
    }
}
